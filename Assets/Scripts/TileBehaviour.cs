using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum TileValue
{
    Minimum, Eigth, Quarter, Half, Full
    , NumTileValues
}

[System.Serializable]
public static class TileColormap
{
    public static Dictionary<TileValue, Color> colorMap;
    static TileColormap()
    {
        colorMap = new Dictionary<TileValue, Color>((int)TileValue.NumTileValues );

        colorMap.Add(TileValue.Minimum, Color.grey);
        colorMap.Add(TileValue.Eigth, new Color(1, 0.2f, 0));
        colorMap.Add(TileValue.Quarter, new Color(1, 0.4f, 0));
        colorMap.Add(TileValue.Half, Color.yellow);
        colorMap.Add(TileValue.Full, Color.green);
    }
    
    
}



public class TileBehaviour : MonoBehaviour
{
    //variables
    TileValue valueAsEnum = TileValue.Minimum;
    float valueAsFloat = 0f;

    //references
    public static MiniGameManager tileGameManager;
    Button button;


    //accessors
    public TileValue ValueEnum {get => valueAsEnum; private set => valueAsEnum = value; }
    public float ValueFloat {get => valueAsFloat; private set => valueAsFloat = value; }


    //functions
    //

    public void SetTileValue(TileValue range, float actualValue)
    {
        //set from to a higher value, or reset to minimum, but don't overload with a lower value
        if(range > valueAsEnum || range == TileValue.Minimum)
        { 
            valueAsEnum = range;
            valueAsFloat = actualValue;
        }
    }

    public void HalveTile()
    {
        if(valueAsEnum > TileValue.Minimum)
        {
            valueAsEnum -= 1;

            valueAsFloat = (valueAsEnum == TileValue.Minimum)? 0.0f: valueAsFloat *0.5f;

            RevealTileValue();
        }
    }


    public void RevealTileValue()
    {
        GetComponent<Image>().color = TileColormap.colorMap[valueAsEnum];
    }

    //return to unchecked, valueless state
    public void ResetTile()
    {
        valueAsEnum = TileValue.Minimum;
        valueAsFloat = 0f;
         GetComponent<Image>().color = Color.white;
    }

    public void SetGameManager(MiniGameManager manager)
    {
        tileGameManager = manager;
    }

    public void Awake()
    {
        useGUILayout = false;
        button = GetComponent<Button>();
    }

    public void OnEnable()
    {
        if(button != null)
            GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    public void OnDisable()
    {
        if(button != null)
            GetComponent<Button>().onClick.RemoveListener(OnClicked);
    }

    void OnClicked()
    {
        if(tileGameManager != null)
        {
            tileGameManager.OnTileClicked(transform.GetSiblingIndex());
        }
    }
}
