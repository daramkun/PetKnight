using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMarquee : MonoBehaviour
{
    private static readonly int Offset = Shader.PropertyToID("_Offset");

    [SerializeField]
    private SpriteRenderer[] stageElements;
    [SerializeField]
    private bool[] stageElementsMustMarquees;
    [SerializeField]
    private bool frontStageBackmasking = false;

    public bool marqueeOn = true;

    private MaterialPropertyBlock[] propertyBlocks;

    void Awake()
    {
        propertyBlocks = new MaterialPropertyBlock[stageElements.Length];
        for (var i = 0; i < stageElements.Length; ++i)
        {
            var propertyBlock = new MaterialPropertyBlock();
            stageElements[i].GetPropertyBlock(propertyBlock);
            propertyBlocks[i] = propertyBlock;
        }
    }

    void Update()
    {
        var moveUnit = 0.1f;
        for (var i = stageElements.Length - 1; i >= 0; --i)
        {
            var innerMoveUnit = moveUnit;
            if (stageElements[i].sortingLayerName == "Front Stage")
            {
                if (frontStageBackmasking)
                    innerMoveUnit = (moveUnit - 0.025f) * Time.deltaTime;
                else
                    innerMoveUnit = (moveUnit - 0.025f) * Time.deltaTime;
            }
            else
            {
                innerMoveUnit = moveUnit * Time.deltaTime;
                moveUnit += 0.025f;
            }

            if (!marqueeOn && !stageElementsMustMarquees[i])
                continue;

            var material = stageElements[i].material;
            var offset = propertyBlocks[i].GetFloat(Offset) + innerMoveUnit;
            if (offset > 1) offset -= 1;
            if (offset < 0) offset += 1;
            propertyBlocks[i].SetFloat(Offset, offset);

            stageElements[i].SetPropertyBlock(propertyBlocks[i]);
        }
    }
}
