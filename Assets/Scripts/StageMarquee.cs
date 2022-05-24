using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMarquee : MonoBehaviour
{
    private static readonly int Offset = Shader.PropertyToID("_Offset");

    [SerializeField]
    private SpriteRenderer[] _stageElements;
    [SerializeField]
    private bool[] _stageElementsMustMarquees;
    [SerializeField]
    private bool _frontStageBackmasking = false;

    public bool _marqueeOn = true;

    private MaterialPropertyBlock[] _propertyBlocks;

    void Awake()
    {
        _propertyBlocks = new MaterialPropertyBlock[_stageElements.Length];
        for (var i = 0; i < _stageElements.Length; ++i)
        {
            var propertyBlock = new MaterialPropertyBlock();
            _stageElements[i].GetPropertyBlock(propertyBlock);
            _propertyBlocks[i] = propertyBlock;
        }
    }

    void Update()
    {
        var moveUnit = 0.1f;
        for (var i = _stageElements.Length - 1; i >= 0; --i)
        {
            var innerMoveUnit = moveUnit;
            if (_stageElements[i].sortingLayerName == "Front Stage")
            {
                if (_frontStageBackmasking)
                    innerMoveUnit = (moveUnit - 0.025f) * Time.deltaTime;
                else
                    innerMoveUnit = (moveUnit - 0.025f) * Time.deltaTime;
            }
            else
            {
                innerMoveUnit = moveUnit * Time.deltaTime;
                moveUnit += 0.025f;
            }

            if (!_marqueeOn && !_stageElementsMustMarquees[i])
                continue;

            var material = _stageElements[i].material;
            var offset = _propertyBlocks[i].GetFloat(Offset) + innerMoveUnit;
            if (offset > 1) offset -= 1;
            if (offset < 0) offset += 1;
            _propertyBlocks[i].SetFloat(Offset, offset);

            _stageElements[i].SetPropertyBlock(_propertyBlocks[i]);
        }
    }
}
