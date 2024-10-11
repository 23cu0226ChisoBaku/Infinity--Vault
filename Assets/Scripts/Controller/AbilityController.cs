using UnityEngine;

public class AbilityController : MonoBehaviour 
{
    private PlayerAbility _ability;

    public void SetAbility(PlayerAbility ability)
    {
        _ability = ability;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _ability?.ActiveAbility();
        }
    }    

    private void OnDestroy()
    {
        
    }
}