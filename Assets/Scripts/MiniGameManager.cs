using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    //tile map variables
    const int rowSize = 32;
      TileBehaviour[,] tiles;

    public const int numMaxTiles = 8;
    Vector2[] maxTileLocations; 

    [SerializeField]
    int minMaxValue = 2000;
    [SerializeField]
    int maxMaxValue = 5000;

    //game mode
    bool inCollectingMode = false;
    [SerializeField]
    Toggle collectToggle;

    //game status
    int collectedGold = 0;
    [SerializeField]
    TMPro.TextMeshProUGUI outgoingMessage;
    [SerializeField]
    TMPro.TextMeshProUGUI collectedGoldText;

    [SerializeField]
    int searchAttempts = 6;
    [SerializeField]
    int collectAttempts = 3;

    [SerializeField]
    TMPro.TextMeshProUGUI searchesText, collectionsText;



    //functions
    //

    private void Start()
    {
        tiles = new TileBehaviour[rowSize,rowSize];

        TileBehaviour[] tilesChildList = transform.GetComponentsInChildren<TileBehaviour>();

        for(int i = 0; i< rowSize; i++)
        {
            for(int j = 0; j < rowSize; j++)
            {
                tiles[i, j] = tilesChildList[(i * rowSize) + j];

            }
        }

        tiles[0,0].SetGameManager(this);

        maxTileLocations = new Vector2[numMaxTiles];
        
        SetUpMiniGame();
    }


    private void OnEnable()
    {
        collectToggle.onValueChanged.AddListener(SwitchModes);
        SetUpMiniGame();
    }

    private void OnDisable()
    {
        collectToggle.onValueChanged.RemoveListener(SwitchModes);
    }

    private void SwitchModes(bool isOn)
    {
        inCollectingMode = isOn;

        string modeText = inCollectingMode ? "collection mode!" : "detection mode!";

        outgoingMessage.text = "Switching to " + modeText;
    }

    void SetUpMiniGame()
    {
        if(tiles != null)
        { 
            SetTileValues();

            Debug.Log("game is ready");
        }
        else
            Debug.Log("could not set up minigame while tiles are invalid");
                
    }
       

    public void OnTileClicked(int childCount)
    {
        Vector2Int coordinate = ChildCountToCoordinates(childCount);

        if(inCollectingMode && collectAttempts > 0)
        { 
            int gold = (int)tiles[coordinate.x, coordinate.y].ValueFloat;
            collectedGold += gold;
            tiles[coordinate.x, coordinate.y].SetTileValue(TileValue.Minimum, 0);

            collectAttempts--;

            outgoingMessage.text = "You Collected $" + gold + " in gold in tile " + coordinate;
            collectedGoldText.text = "Gold Collected " + collectedGold;
            collectionsText.text = "Collection Attempts: " + collectAttempts;
        }
        else if(!inCollectingMode && searchAttempts > 0)
        {

            ScanTiles(coordinate.y, coordinate.x); 
            searchAttempts--;
            searchesText.text = "Detection Attempts: " + searchAttempts;

            outgoingMessage.text = "Searching Coordinate " + coordinate; 
        }
    }

    Vector2Int ChildCountToCoordinates(int childCount)
    {
        Vector2Int coordinate = Vector2Int.zero;
        coordinate.y = childCount % rowSize;
        coordinate.x = (int)(childCount /rowSize);

        return coordinate;
    }

    void SetTileValues()
    {
        for(int i = 0; i < numMaxTiles; i++)
        {
            int x = Random.Range(0, rowSize);
            int y = Random.Range(0, rowSize);

            if(tiles[x,y].ValueEnum != TileValue.Full)
            {
                maxTileLocations[i] = new Vector2(x,y);

                int randomAmount = Random.Range(minMaxValue, maxMaxValue);
                tiles[x, y].SetTileValue(TileValue.Full, randomAmount);

                SetSurroundingTileValues(x, y, randomAmount);
            }
            else //if it tries to place a full square on another full square, try it again;
                i--;
        }
    }

   void SetSurroundingTileValues(int row, int column, float centerTileValue)
    {

        float gold = centerTileValue;
        //n = number of tiles away from the center
        int n = 0;
        float multiplier = 0.5f;

        //loop every level of distance, starting with the half
        for(int i = (int)TileValue.Half ; i > (int)TileValue.Minimum; i--)
        {
            n++;
            multiplier *= 0.5f;
            gold = centerTileValue * multiplier;

            //go through each row
            for(int y = row -n; y <= row + n ; y++)
            {
                //skipping rows that are out of bounds
                if(y < 0 || y >= rowSize) 
                    continue;

                //set the outer columns of the middle rows
                if(Mathf.Abs(y) != n) 
                {
                    if(n + column < rowSize)
                        tiles[column + n, y ].SetTileValue((TileValue)i, gold);
                    if(column - n >= 0 )
                        tiles[column - n, y ].SetTileValue((TileValue)i, gold);
                }
                //and each column of the top and bottom
                else 
                { 
                    for(int x = column -n; x <= column + n; x++)
                    {
                        if(x < 0 || x >= rowSize)
                            continue;
                        
                        tiles[x,y].SetTileValue((TileValue)i, gold);
                    }
                }
            }
        }
    }


    //reveal the selected square and the 8 tiles surrounding it
    void ScanTiles(int row, int column)
    {
        //n = max distance from the center
        int n = 1;

        for(int i = row - n; i <= row + n; i++)
        {
            if(i < 0 || i >= rowSize)
                continue;

            for(int j = column - n; j <= column + n; j++)
            {
                if( j < 0 || j >= rowSize)
                    continue;

                tiles[j,i].RevealTileValue();

            }
        }
    }


}
