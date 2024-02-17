using UnityEngine;

public class GiantFight : MonoBehaviour
{
    [SerializeField] private GameObject _giant;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _giantFightTrigger;
    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // temp to teleport to giant
        {
            _player.transform.position = _giantFightTrigger.transform.position + new Vector3(2, 1, 0);
        }
    }
}
