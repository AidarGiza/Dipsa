using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int highscore;

    private string saveFolder;

    public int Coins
    {
        get => coins;
        set
        {
            SaveFile(value);
            coins = value;
        }
    }
    private int coins;

    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        saveFolder = Application.persistentDataPath;
#else
        saveFolder = Application.dataPath;
#endif

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        highscore = LoadScore();
        LoadFile();
    }
    
    public void SaveScore(int score)
    {
        BinaryWriter bw;
        highscore = score;
        //create the file
        try
        {
            bw = new BinaryWriter(new FileStream(Path.Combine(saveFolder, "Score"), FileMode.Create));
        }
        catch (IOException e)
        {
            Debug.Log(e.Message + "\n Cannot create file.");
            return;
        }

        //writing into the file
        try
        {
            bw.Write(score);

        }
        catch (IOException e)
        {
            Debug.Log(e.Message + "\n Cannot write to file.");
            return;
        }
        bw.Close();
    }

    public int LoadScore()
    {
        BinaryReader br;
        int score;
        //reading from the file
        try
        {
            br = new BinaryReader(new FileStream(Path.Combine(saveFolder, "Score"), FileMode.Open));
        }
        catch (IOException e)
        {
            Debug.Log(e.Message + "\n Cannot open file.");
            return 0;
        }

        try
        {
            score = br.ReadInt32();
        }
        catch (IOException e)
        {
            Debug.Log(e.Message + "\n Cannot read from file.");
            score = 0;
        }
        br.Close();
        return score;
    }

    public void SaveFile(int val)
    {
        BinaryWriter bw;
        //create the file
        try
        {
            bw = new BinaryWriter(new FileStream(Path.Combine(saveFolder, "Profile"), FileMode.Create));
        }
        catch (IOException e)
        {
            Debug.Log(e.Message + "\n Cannot create file.");
            return;
        }

        //writing into the file
        try
        {
            bw.Write(val);

        }
        catch (IOException e)
        {
            Debug.Log(e.Message + "\n Cannot write to file.");
            return;
        }
        bw.Close();
    }

    public void LoadFile()
    {
        BinaryReader br;
        //reading from the file
        try
        {
            br = new BinaryReader(new FileStream(Path.Combine(saveFolder, "Profile"), FileMode.Open));
        }
        catch (IOException e)
        {
            Debug.Log(e.Message + "\n Cannot open file.");
            coins = 0;
            return;
        }

        try
        {
            coins = br.ReadInt32();
        }
        catch (IOException e)
        {
            Debug.Log(e.Message + "\n Cannot read from file.");
            coins = 0;
        }
        br.Close();
    }
}
