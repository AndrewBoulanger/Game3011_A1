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
        colorMap.Add(TileValue.Eigth, Color.cyan);
        colorMap.Add(TileValue.Quarter, Color.blue);
        colorMap.Add(TileValue.Half, Color.green);
        colorMap.Add(TileValue.Full, Color.yellow);
    }
    
    
}



public class TileBehaviour : MonoBehaviour
{
    //variables
    TileValue valueAsEnum = TileValue.Minimum;
    bool isChecked = false;
    float valueAsFloat = 0f;

    //references
    public static MiniGameManager tileGameManager;
    Button button;


    //accessors
    public TileValue ValueEnum {get => valueAsEnum; private set => valueAsEnum = value; }
    public float ValueFloat {get => valueAsFloat; private set => valueAsFloat = value; }
    public bool WasChecked {get => isChecked; private set => isChecked = value;}


    //functions
    //

    public void SetTileValue(TileValue range, float actualValue)
    {
        //set from to a higher value, or reset to minimum, but don't overload with a lower value
        if(range > valueAsEnum || range == TileValue.Minimum)
        { 
            valueAsEnum = range;
            valueAsFloat = actualValue;

            //if its already been revealed, you can change the tile color, (cases: when a revealed tile is being collected)
            if(isChecked)
                GetComponent<Image>().color = TileColormap.colorMap[valueAsEnum];
        }
    }

    public void RevealTileValue()
    {
        isChecked = true;
        GetComponent<Image>().color = TileColormap.colorMap[valueAsEnum];
    }

    //return to unchecked, valueless state
    public void ResetTile()
    {
        valueAsEnum = TileValue.Minimum;
        isChecked = false;
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
