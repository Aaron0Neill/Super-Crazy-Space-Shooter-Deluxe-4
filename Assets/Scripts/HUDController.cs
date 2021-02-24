using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    static HUDController _instance;

    TMP_Text _score;
    TMP_Text _lives;
    TMP_Text _target;
    TMP_Text _multiplier;
    TMP_Text _highscore;
    

    // AWAKE ** simple brief for ben ** 

    /*
     Description
    Awake is called when the script instance is being loaded.

    Awake is called either when an active GameObject that contains the script is initialized when a Scene loads, 
    or when a previously inactive GameObject is set to active, or after a GameObject created with Object.
    Instantiate is initialized. Use Awake to initialize variables or states before the application starts.

    Unity calls Awake only once during the lifetime of the script instance. A script's lifetime lasts until the Scene that contains it is unloaded.
    If the Scene is loaded again, Unity loads the script instance again, so Awake will be called again. If the Scene is loaded multiple times additively,
    Unity loads several script instances, so Awake will be called several times (one on each instance).
    For active GameObjects placed in a Scene, Unity calls Awake after all active GameObjects in the Scene are initialized,
    so you can safely use methods such as GameObject.FindWithTag to query other GameObjects.
    The order that Unity calls each GameObject's Awake is not deterministic. Because of this,
    you should not rely on one GameObject's Awake being called before or after another
    (for example, you should not assume that a reference set up by one GameObject's Awake will be usable in another GameObject's Awake).
    Instead, you should use Awake to set up references between scripts, and use Start, which is called after all Awake calls are finished,
    to pass any information back and forth.
    Awake is always called before any Start functions. This allows you to order initialization of scripts.
    Awake is called even if the script is a disabled component of an active GameObject.
    Awake can not act as a coroutine.

    Note: Use Awake instead of the constructor for initialization, as the serialized state of the component is undefined at construction time. Awake is called once, just like the constructor. 
     */
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _lives = GameObject.Find("HUD/Lives").GetComponent<TMP_Text>();
        _score = GameObject.Find("HUD/Score/ScoreText").GetComponent<TMP_Text>(); // store the transform of the score object 
        _target = GameObject.Find("HUD/Target/TargetText").GetComponent<TMP_Text>(); // store the transform of the target object 
        _multiplier = GameObject.Find("HUD/Multiplier/MultiplierText").GetComponent<TMP_Text>(); // store the transform of the multiplier object 
        _highscore = GameObject.Find("HUD/HighScore/HighScoreText").GetComponent<TMP_Text>(); // store the transform of the highscore object 

        _lives.text = "x 3"; // Give the lives a default value
        _score.text = "0"; // Give the score a default value
        _target.text = "0"; // Give the target a default value
        _multiplier.text = "X0"; // Give the multiplier a default value
        _highscore.text = "0"; // Give the highscore a default value
    }

    //void OnDisable()
    //{
    //    if (_instance != null)
    //        _instance = null;    
    //}

    /// <summary>
    /// Updates the text on the HUD that relates to the score that the player has.
    /// Sets the number to whatever is passed into the function
    /// </summary>
    /// <param name="t_score">The players overall score</param>
    public void UpdateScore(int t_score)
    {
        _score.text = t_score.ToString();
    }

    /// <summary>
    /// Updates the text on the HUD regarding the number of lives left
    /// Sets the number to be whatever is passed into it.
    /// </summary>
    /// <param name="t_lives">Number of lives the player has left</param>
    public void UpdateLives(int t_lives)
    {
        _lives.text = "x " + t_lives;
    }

    /// <summary>
    /// Updated the HUD regarding the score target the player is aiming for
    /// Sets the number to a higer amount when the player achieves the previouse target
    /// </summary>
    /// <param name="t_score"></param>
    public void UpdateTarget(int t_score)
    {
        _target.text = t_score.ToString(); // Setting the text to be 500,000 more than the previous target
    }

    public void UpdateMultiplier(float t_multiplier)
    {
        _multiplier.text = t_multiplier.ToString();
    }

    public void UpdateHighScore(float t_highscore)
    {
        _highscore.text = t_highscore.ToString();
    }
}
