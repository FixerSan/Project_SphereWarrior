using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class Managers : Singleton<Managers>
{
    private ObjectManager _object;
    private ResourceManager _resource;
    private UIManager _ui;
    private PoolManager _pool;
    private InputManager _input;
    private SceneManager _scene;
    private GridManager _grid;
    private DataManager _data;
    private FaceManager _face;
    private BallManager _ball;

    private GameManager _game;

    private static bool init = false;

    public static ObjectManager Object { get { return Instance?._object; } }
    public static GameManager Game { get { return Instance?._game; } }
    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static UIManager UI { get { return Instance?._ui; } }
    public static PoolManager Pool { get { return Instance?._pool; } }
    public static InputManager Input { get { return Instance?._input; } }
    public static SceneManager Scene { get { return Instance?._scene; } }
    public static GridManager Grid { get { return Instance?._grid; } }
    public static DataManager Data { get { return Instance?._data; } }
    public static FaceManager Face { get { return Instance?._face; } }
    public static BallManager Ball { get { return Instance?._ball; } }


    private void Awake()
    {
        Init();
    }

    private static void Init()
    {
        if (init) return;
        Instance._object = new ObjectManager();
        Instance._resource = new ResourceManager();
        Instance._ui = new UIManager();
        Instance._pool = new PoolManager();
        Instance._input = new InputManager();
        Instance._scene = new SceneManager();
        Instance._grid = new GridManager();
        Instance._data = new DataManager();
        Instance._face = new FaceManager();
        Instance._ball = new BallManager();

        Instance._game = GameManager.Instance;
        init = true;
    }
}
