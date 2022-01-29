using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellsContent : MonoBehaviour
{

    [SerializeField] private RectTransform _zoomTargetRt;
    private readonly float _ZOOM_IN_MAX = 3.5f;
    private readonly float _ZOOM_OUT_MAX = 0.95f;
    private readonly float _ZOOM_SPEED = 1.5f;

    private bool _isZooming = false;

    public GameObject Cell_prefab;
    private GameObject[,] cell_object;
    private int[,] cell_value_by_computer;

    private int stage_level;
    private int level;
    private int hint_cnt;

    private int Clicked_Cell_x = -1, Clicked_Cell_y = -1;

    public GameObject NumberButtonGroup_object;

    public bool AssistantMode = true;

    public GameObject GameCanvas_object;

    void Start()
    {
        stage_level = PlayerPrefs.GetInt("Level", 3);
        hint_cnt = PlayerPrefs.GetInt("Hint", 16);
        //stage_level = 4;
        level = stage_level * stage_level;
        //hint_cnt = level * level - 1;

        _zoomTargetRt = transform.GetComponent<RectTransform>();
        MakeSudoku();
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            ZoomAndPan();
        }
        else
        {
            _isZooming = false;
        }
    }

    #region zoom
    private void ZoomAndPan()
    {
        if (_isZooming == false)
        {
            _isZooming = true;
        }

        /* get zoomAmount */
        var prevTouchAPos = Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition;
        var prevTouchBPos = Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition;
        var curTouchAPos = Input.GetTouch(0).position;
        var curTouchBPos = Input.GetTouch(1).position;
        var deltaDistance =
            Vector2.Distance(Normalize(curTouchAPos), Normalize(curTouchBPos))
            - Vector2.Distance(Normalize(prevTouchAPos), Normalize(prevTouchBPos));
        var currentScale = _zoomTargetRt.localScale.x;
        var zoomAmount = deltaDistance * currentScale * _ZOOM_SPEED; // zoomAmount == deltaScale

        /* clamp & zoom */
        var zoomedScale = currentScale + zoomAmount;
        if (zoomedScale < _ZOOM_OUT_MAX)
        {
            zoomedScale = _ZOOM_OUT_MAX;
        }
        if (_ZOOM_IN_MAX < zoomedScale)
        {
            zoomedScale = _ZOOM_IN_MAX;
        }
        _zoomTargetRt.localScale = zoomedScale * Vector3.one;

    }
    private Vector2 Normalize(Vector2 position)
    {
        var normlizedPos = new Vector2(
            (position.x - Screen.width * 0.5f) / (Screen.width * 0.5f),
            (position.y - Screen.height * 0.5f) / (Screen.height * 0.5f));
        return normlizedPos;
    }

    #endregion

    #region make initial sudoku
    private void MakeSudoku()
    {
        MakeCells();
        SetInitCellValue();

        for (int i = 0; i < 30; i++)
        {
            while (!MixSudokuCol()) ;
            while (!MixSudokuRow()) ;
        }

        MixNum();
        for (int i = 0; i < level; i++)
        {
            for (int j = 0; j < level; j++)
            {
                cell_value_by_computer[i, j]++;
            }
        }
        SetHint();
    }
    private void MakeCells()
    {
        // make cells
        transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2((1000 + (10 * (level - 1))) / level, (1000 + (10 * (level - 1))) / level);
        transform.GetComponent<GridLayoutGroup>().constraintCount = level;

        cell_object = new GameObject[level, level];
        for (int i = 0; i < level; i++)
        {
            for (int j = 0; j < level; j++)
            {
                cell_object[i, j] = Instantiate(Cell_prefab) as GameObject;
                cell_object[i, j].transform.SetParent(transform, false);
                cell_object[i, j].GetComponent<Cell>().Init(stage_level, i, j);
            }
        }

        // make bold(dark) line
        int st = 0;
        int end = stage_level - 1;
        for (int i = 0; i < stage_level; i++)
        {
            for (int j = 0; j < level; j++)
            {
                cell_object[st, j].GetComponent<Cell>().ShowTopBar();
                cell_object[end, j].GetComponent<Cell>().ShowBottomBar();
                cell_object[j, st].GetComponent<Cell>().ShowLeftBar();
                cell_object[j, end].GetComponent<Cell>().ShowRightBar();
            }
            st += stage_level;
            end += stage_level;
        }
    }
    private void SetInitCellValue()
    {
        cell_value_by_computer = new int[level, level];
        int tmp = -1;
        for (int i = 0; i < level; i++)
        {
            if (i % stage_level == 0) tmp++;
            for (int j = 0; j < level; j++)
            {
                cell_value_by_computer[i, j] = ((i * stage_level) + j + tmp) % level;
            }
        }
    }
    private bool MixSudokuRow()
    {
        int rand1 = Random.Range(0, stage_level);
        int rand2 = Random.Range(0, stage_level);
        int rand3 = Random.Range(0, stage_level);
        if (rand2 == rand3) return false;

        int mixrow1 = rand1 * stage_level + rand2;
        int mixrow2 = rand1 * stage_level + rand3;

        for (int i = 0; i < level; i++)
        {
            int tmp = cell_value_by_computer[mixrow1, i];
            cell_value_by_computer[mixrow1, i] = cell_value_by_computer[mixrow2, i];
            cell_value_by_computer[mixrow2, i] = tmp;
        }
        return true;
    }
    private bool MixSudokuCol()
    {
        int rand1 = Random.Range(0, stage_level);
        int rand2 = Random.Range(0, stage_level);
        int rand3 = Random.Range(0, stage_level);
        if (rand2 == rand3) return false;

        int mixrow1 = rand1 * stage_level + rand2;
        int mixrow2 = rand1 * stage_level + rand3;

        for (int i = 0; i < level; i++)
        {
            int tmp = cell_value_by_computer[i, mixrow1];
            cell_value_by_computer[i, mixrow1] = cell_value_by_computer[i, mixrow2];
            cell_value_by_computer[i, mixrow2] = tmp;
        }
        return true;
    }
    private void MixNum()
    {
        int[] num = new int[level];
        for (int i = 0; i < level; i++)
        {
            num[i] = i;
        }
        for (int i = 0; i < level; i++)
        {
            int tmp = num[i];
            int rand = Random.Range(0, level);
            num[i] = num[rand];
            num[rand] = tmp;
        }

        for (int i = 0; i < level; i++)
        {
            for (int j = 0; j < level; j++)
            {
                cell_value_by_computer[i, j] = num[cell_value_by_computer[i, j]];
            }
        }
    }
    private void SetHint()
    {
        int level_pow = level * level;

        int[] cell_idx_rand = new int[level_pow];
        for (int i = 0; i < cell_idx_rand.Length; i++)
        {
            cell_idx_rand[i] = i;
        }
        for (int i = 0; i < cell_idx_rand.Length; i++)
        {
            int tmp = cell_idx_rand[i];
            int rand = Random.Range(0, level_pow);
            cell_idx_rand[i] = cell_idx_rand[rand];
            cell_idx_rand[rand] = tmp;
        }

        for (int i = 0; i < hint_cnt; i++)
        {
            int row = cell_idx_rand[i] / level;
            int col = cell_idx_rand[i] % level;
            cell_object[row, col].GetComponent<Cell>().SetValueByComputer(cell_value_by_computer[row, col]);
        }
    }

    #endregion


    public void ClickCell(int line_x, int line_y, int value, bool[] in_note)
    {
        if (Clicked_Cell_x >= 0)
        {
            cell_object[Clicked_Cell_x, Clicked_Cell_y].GetComponent<Cell>().EndClick();
        }
        Clicked_Cell_x = line_x;
        Clicked_Cell_y = line_y;

        NumberButtonGroup_object.GetComponent<NumberButtonGroup>().SetAllValues(value, in_note);
    }

    public void InsertValueToCell(int val, bool notemode)
    {
        if (Clicked_Cell_x < 0) return;
        if (notemode)
        {
            cell_object[Clicked_Cell_x, Clicked_Cell_y].GetComponent<Cell>().SetToggleNote(val);
        }
        else
        {
            if (cell_object[Clicked_Cell_x, Clicked_Cell_y].GetComponent<Cell>().GetValue() == val)
            {
                cell_object[Clicked_Cell_x, Clicked_Cell_y].GetComponent<Cell>().SetValueInit();
            }
            else
            {
                cell_object[Clicked_Cell_x, Clicked_Cell_y].GetComponent<Cell>().SetValueByPlayer(val);
            }
        }

        if (!notemode)
        {
            BlindHintRow(Clicked_Cell_x);
            BlindHintCol(Clicked_Cell_y);
            BlindHintSection((int)(Clicked_Cell_x / stage_level), (int)(Clicked_Cell_y / stage_level));

            ShowHintRow(Clicked_Cell_x);
            ShowHintCol(Clicked_Cell_y);
            ShowHintSection((int)(Clicked_Cell_x / stage_level), (int)(Clicked_Cell_y / stage_level));

            CheckCompleteGame();
        }
    }

    #region Show Hint
    private void ShowHintRow(int row)
    {
        int[] value_cnt = new int[level + 1];
        for (int i = 1; i <= level; i++)
        {
            value_cnt[i] = 0;
        }
        for (int i = 0; i < level; i++)
        {
            int v = cell_object[row, i].GetComponent<Cell>().GetValue();
            if (v == -1)
            {
                value_cnt[cell_value_by_computer[row, i]]++;
            }
            else if (v > 0)
            {
                value_cnt[v]++;
            }
        }
        for (int i = 0; i < level; i++)
        {
            int v = cell_object[row, i].GetComponent<Cell>().GetValue();
            if (v > 0)
            {
                if (value_cnt[v] > 1)
                {
                    cell_object[row, i].GetComponent<Cell>().ShowWrong();
                }
            }
        }
    }
    private void ShowHintCol(int col)
    {
        int[] value_cnt = new int[level + 1];
        for (int i = 1; i <= level; i++)
        {
            value_cnt[i] = 0;
        }
        for (int i = 0; i < level; i++)
        {
            int v = cell_object[i, col].GetComponent<Cell>().GetValue();
            if (v == -1)
            {
                value_cnt[cell_value_by_computer[i, col]]++;
            }
            else if (v > 0)
            {
                value_cnt[v]++;
            }
        }
        for (int i = 0; i < level; i++)
        {
            int v = cell_object[i, col].GetComponent<Cell>().GetValue();
            if (v > 0)
            {
                if (value_cnt[v] > 1)
                {
                    cell_object[i, col].GetComponent<Cell>().ShowWrong();
                }
            }
        }
    }
    private void ShowHintSection(int row_main, int col_main)
    {
        int[] check_flg = new int[level + 1];
        for (int i = 1; i <= level; i++)
        {
            check_flg[i] = 0;
        }

        for (int i = 0; i < stage_level; i++)
        {
            for (int j = 0; j < stage_level; j++)
            {
                int row = i + (row_main * stage_level);
                int col = j + (col_main * stage_level);

                int v = cell_object[row, col].GetComponent<Cell>().GetValue();
                if (v == -1)
                {
                    check_flg[cell_value_by_computer[row, col]]++;
                }
                else if (v > 0)
                {
                    check_flg[v]++;
                }
            }
        }
        for (int i = 0; i < stage_level; i++)
        {
            for (int j = 0; j < stage_level; j++)
            {
                int row = i + (row_main * stage_level);
                int col = j + (col_main * stage_level);

                int v = cell_object[row, col].GetComponent<Cell>().GetValue();
                if (v > 0)
                {
                    if (check_flg[v] > 1)
                    {
                        cell_object[row, col].GetComponent<Cell>().ShowWrong();
                    }
                }
            }
        }
    }
    #endregion

    #region Blind Hint
    private void BlindHintRow(int row)
    {
        for (int i = 0; i < level; i++)
        {
            cell_object[row, i].GetComponent<Cell>().BlindWrong();
        }
    }
    private void BlindHintCol(int col)
    {
        for (int i = 0; i < level; i++)
        {
            cell_object[i, col].GetComponent<Cell>().BlindWrong();
        }
    }
    private void BlindHintSection(int row_main, int col_main)
    {
        for (int i = 0; i < stage_level; i++)
        {
            for (int j = 0; j < stage_level; j++)
            {
                int row = i + (row_main * stage_level);
                int col = j + (col_main * stage_level);
                cell_object[row, col].GetComponent<Cell>().BlindWrong();
            }
        }
    }
    #endregion
    public void TurnOnAssistant()
    {
        AssistantMode = true;
        for (int i = 0; i < level; i++)
        {
            ShowHintCol(i);
            ShowHintRow(i);
        }
        for (int i = 0; i < stage_level; i++)
        {
            for (int j = 0; j < stage_level; j++)
            {
                ShowHintSection(i, j);
            }
        }
    }
    public void TurnOffAssitant()
    {
        AssistantMode = false;

        for (int i = 0; i < level; i++)
        {
            BlindHintCol(i);
            BlindHintRow(i);
        }
        for (int i = 0; i < stage_level; i++)
        {
            for (int j = 0; j < stage_level; j++)
            {
                BlindHintSection(i, j);
            }
        }
    }

    public void ToggleAssistantMode()
    {
        if (AssistantMode) TurnOffAssitant();
        else TurnOnAssistant();
    }
    private void CheckCompleteGame()
    {
        for (int i = 0; i < level; i++)
        {
            for (int j = 0; j < level; j++)
            {
                if (!cell_object[i, j].GetComponent<Cell>().correct_value) return;
            }
        }
        GameCanvas_object.GetComponent<GameCanvas>().CompleteGame();
    }
}
