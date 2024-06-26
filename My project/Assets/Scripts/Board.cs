
using UnityEngine;

public class Board : MonoBehaviour
{
    private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[]
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H,
        KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P,
        KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z,
    };
    private Row[] rows;

    private string[] solutions;
    private string[] validWords;
    private int rowIndex;
    private int columnIndex;
    private string word;

    [Header("States")]
    public Tile.State emptyState;
    public Tile.State occupiedStates;
    public Tile.State correctStates;
    public Tile.State wrongSpotState;
    public Tile.State inCorrectState;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();  
    }
    private void Start()
    {
        LoadData();
        SetRandomWord();
    }
    private void LoadData()
    {
        TextAsset textFile=  Resources.Load("new") as TextAsset;
        validWords = textFile.text.Split("\n");

        textFile = Resources.Load("words") as TextAsset;
        solutions = textFile.text.Split("\n");
    }
    private void SetRandomWord()
    {
        word = solutions[Random.Range(0, solutions.Length)];
        word = word.ToLower().Trim();
    }
    void Update()
    {
        Row currentRow = rows[rowIndex];
        if(Input.GetKeyDown(KeyCode.Backspace)) 
        {
            columnIndex =Mathf.Max(columnIndex-1, 0);
            currentRow.tiles[columnIndex].SetLetter('\0');
            currentRow.tiles[columnIndex].SetState(emptyState);
        }
        else if (columnIndex >= currentRow.tiles.Length)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SubmitRow(currentRow);
            }
        }
        else
        {
            for (int i = 0; i < SUPPORTED_KEYS.Length; i++)
            {
                if (Input.GetKeyDown(SUPPORTED_KEYS[i]))
                {
                    currentRow.tiles[columnIndex].SetLetter((char)SUPPORTED_KEYS[i]);
                    currentRow.tiles[columnIndex].SetState(occupiedStates);
                    columnIndex++;
                    break;
                }
            }
        }
    }
    private void SubmitRow(Row row)
    {
       for(int i=0; i < row.tiles.Length; i++)
       {
            Tile tile = row.tiles[i];
            if (tile.letter == word[i])
            {
                tile.SetState(correctStates);
            }else if (word.Contains(tile.letter))
            {
                tile.SetState(wrongSpotState);
            }
            else
            {
                tile.SetState(inCorrectState);
            }
       }
        rowIndex++;
        columnIndex= 0;
        if (rowIndex >= rows.Length)
        {
            enabled = false;
        }
    }
}
