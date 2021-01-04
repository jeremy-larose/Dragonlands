using UnityEngine;

public class AttackedDebug : MonoBehaviour, IAttackable
{
    public void OnAttack(GameObject attacker, Attack attack)
    {
        //TODO: Bug with character weapon where hits himself for lots of damage. Need to figure out what the workflow is for 
        // adding new weapons and assigning them to characters. Also need to refine NPCBase, NPCController, Character, and PlayerController
        // scripts to get rid of redundancies and clean up code. Will back up prior.
        if (attack.IsCritical)
            Debug.Log("CRITICAL HIT!");

        Debug.Log($"{attacker.name} attacked {name} for {attack.Damage}");
    }
}