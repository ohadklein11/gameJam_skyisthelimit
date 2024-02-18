using Giant;
using UnityEngine;

public class GiantFightManager : Singleton<MonoBehaviour>
{
    [SerializeField] private GiantBehavior giantBehavior;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _giantFightTrigger;
    [SerializeField] private GameObject _stonesTrigger;

    [SerializeField] private PathToGoose _pathToGoose;
    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // temp to teleport to giant
        {
            _player.transform.position = _giantFightTrigger.transform.position + new Vector3(-2, 1, 0);
        }
    }

    public void StartGiantFight()
    {
        giantBehavior.StartGiantFight();
    }
    
    public void EndGiantFight()
    {
        _pathToGoose.gameObject.SetActive(true);
        _stonesTrigger.SetActive(true);

    }
}
