using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets._2D;

public class StackGUI : MonoBehaviour
{
    private RectTransform _rectTransform;
    
    [SerializeField] private PlayerGUI playerGUI;
    [SerializeField] private GameObject memoryBrick;
    [SerializeField] private RectTransform bricksParent;
    [SerializeField] private RectTransform memoryOverlay;

    public List<string> m_actionName = new List<string>();
    public List<GameObject> m_module = new List<GameObject>();

    private List<Image> bricks = new List<Image>();

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        playerGUI.GetPlayerStack().OnStackCompositionUpdate += CreateStackBricks;
        playerGUI.GetPlayerStack().OnStackMemoryUpdate += UpdateMemoryOverlay;

        UpdateMemoryOverlay();
    }


    void UpdateBricksState()
    {
        for (int i = 0; i < bricks.Count; i++)
        {
            if (playerGUI.GetPlayerStack().actions[i].isHacked)
                bricks[i].color = Color.black;
            else if (playerGUI.GetPlayerStack().actions[i].IsEnabled)
                bricks[i].color = playerGUI.GetPlayerStack().actions[i].color;
            else
            {
                Color col = playerGUI.GetPlayerStack().actions[i].color;
                col.r -= 0.2f;
                if (col.r < 0)
                    col.r = 0;

                col.g -= 0.2f;
                if (col.g < 0)
                    col.g = 0;

                col.b -= 0.2f;
                if (col.b < 0)
                    col.b = 0;

                bricks[i].color = col;
            }
        }
    }

    void UpdateMemoryOverlay()
    {
        float height = _rectTransform.rect.height / playerGUI.GetPlayerStack().stackMemoryThreshold * playerGUI.GetPlayerStack().actualMemory;
        height = _rectTransform.rect.height - height;

        memoryOverlay.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        UpdateBricksState();
    }

    void CreateStackBricks()
    {
        foreach (Image img in bricks)
        {
            if(img)
            Destroy(img.gameObject);
        }

        bricks.Clear();

        float decal = 0.0f;

        foreach (Action action in playerGUI.GetPlayerStack().actions)
        {
            if (!action)
                continue;

            GameObject brick = null;

            for (int index = 0; index < m_actionName.Count; ++index)
            {
                if(m_actionName[index] == action.name)
                {
                    brick = Instantiate(m_module[index]);
                    break;
                }
            }

            brick.transform.SetParent(bricksParent, false);

            Image brickImage = brick.GetComponent<Image>();
            brickImage.color = action.color;

            RectTransform brickRectTransform = brickImage.GetComponent<RectTransform>();

            float brickHeight = _rectTransform.rect.height / playerGUI.GetPlayerStack().stackMemoryThreshold * action.MemoryCost;
            brickRectTransform.sizeDelta = new Vector2(_rectTransform.rect.width, brickHeight);

            brickRectTransform.localPosition = new Vector3(0, decal, 0);
            
            bricks.Add(brickImage);

            decal += brickHeight;
        }
        
        UpdateBricksState();
    }

    private void OnDestroy()
    {
        if (playerGUI.IsValidPlayer())
        {
            playerGUI.GetPlayerStack().OnStackCompositionUpdate -= CreateStackBricks;
            playerGUI.GetPlayerStack().OnStackMemoryUpdate -= UpdateMemoryOverlay;
        }
    }
}
