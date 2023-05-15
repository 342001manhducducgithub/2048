using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TileBoard board;
    public CanvasGroup gameOver;
    public CanvasGroup youWin;
    public CanvasGroup congrats;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;
    private int score;
    public bool isContinue;
    private int[,] boards = new int[4, 4]; //
    private void Start()
    {
        NewGame();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    public void NewGame()
    {
        // reset diem
        SetScore(0);
        hiscoreText.text = LoadHiscore().ToString();

        // giau man hinh game over
        gameOver.alpha = 0f;
        gameOver.interactable = false;

        // giau man hinh you win
        youWin.alpha = 0f;
        youWin.interactable = false;

        // giau man hinh congrats
        congrats.alpha = 0f;
        congrats.interactable = false;

        // update board state
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;

        SoundManager.Instance.OffMusicWin();
        SoundManager.Instance.OffMusicLose();
        SoundManager.Instance.OnBackGroundMusic();
        SoundManager.Instance.OffMusicCongrats();
    }
    public void GameOver()
    {
        board.enabled = false;
        gameOver.interactable = true;

        SoundManager.Instance.OnMusicLose();
        SoundManager.Instance.OffBackGroundMusic();

        StartCoroutine(Fade(gameOver, 1f, 1f));
    }
    public void YouWin()
    {
        board.enabled = false;
        youWin.interactable = true;

        SoundManager.Instance.OnMusicWin();
        SoundManager.Instance.OffBackGroundMusic();

        StartCoroutine(Fade(youWin, 1f, 1f));
    }
    public void Congrats()
    {
        board.enabled = false;
        congrats.interactable = true;

        SoundManager.Instance.OnMusicCongrats();
        SoundManager.Instance.OffBackGroundMusic();

        StartCoroutine(Fade(congrats, 1f, 1f));
    }
    public void CountinueGame()
    {
        board.enabled = true;
        youWin.alpha = 0f;
        youWin.interactable = false;

        SoundManager.Instance.OffMusicWin();
        //SoundManager.Instance.OffMusicLose();
        SoundManager.Instance.OnBackGroundMusic();

        isContinue = true;
    }
    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }
    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();

        SaveHiscore();
    }
    private void SaveHiscore()
    {
        int hiscore = LoadHiscore();

        if (score > hiscore)
        {
            PlayerPrefs.SetInt("hiscore", score);
        }
    }
    private int LoadHiscore()
    {
        return PlayerPrefs.GetInt("hiscore", 0);
    }
    public void SaveGame() //
    {
        if (board.enabled)
        {
            string saveTiles = "";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int tileNumber = board.grid.rows[i].cells[j].tile != null ? board.grid.rows[i].cells[j].tile.number : 0;
                    saveTiles += tileNumber.ToString() + ",";
                }
            }
            PlayerPrefs.SetString("Tile", saveTiles);
        }
    }
    public void LoadGame() //
    {
        if (board.enabled)
        {
            string savedTiles = PlayerPrefs.GetString("Tile");

            if (!string.IsNullOrEmpty(savedTiles))
            {
                string[] tileValues = savedTiles.Split(',');

                for (int i = 0; i < board.grid.rows.Length; i++)
                {
                    for (int j = 0; j < board.grid.rows[i].cells.Length; j++)
                    {
                        int tileNumber = int.Parse(tileValues[i * board.grid.rows.Length + j]);

                        if (tileNumber != 0)
                        {
                            board.grid.SpawnTileAt(tileNumber, i, j);
                        }
                        else
                        {
                            board.grid.RemoveTileAt(i, j);
                        }
                    }
                }
            }
        }
    }

}
