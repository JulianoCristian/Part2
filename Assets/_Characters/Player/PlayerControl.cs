﻿using UnityEngine;
using System.Collections;
using RPG.CameraUI; // for mouse events

namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour
    {
        Character character;
        SpecialAbilities abilities;
        WeaponSystem weaponSystem;

        int desiredSpecialAbility = -1;
        bool shouldAttack;
        bool wantsToMove = false;
        Vector3 desiredLocation;
        GameObject target;

        void Start()
        {
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();
            
            RegisterForMouseEvents();
        }

        private void RegisterForMouseEvents()
        {
            var cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

        void Update()
        {
            ScanForAbilityKeyDown();

            weaponSystem.StopAttacking();
            character.ClearDestination();

            if (desiredSpecialAbility != -1)
            {
                if (target != null)
                {
                    if (!IsTargetInRange(target))
                    {
                        character.SetDestination(target.transform.position);
                    }
                    else
                    {
                        AttemptAbility(target);
                    }
                }
                else
                {
                    AttemptAbility();
                }
            }
            else if (target)
            {
                if (!IsTargetInRange(target))
                {
                    character.SetDestination(target.transform.position);
                }
                else
                {
                    weaponSystem.AttackTarget(target);
                }
            }
            else if (wantsToMove)
            {
                character.SetDestination(desiredLocation);
            }
        }

        private void AttemptAbility(GameObject target = null)
        {
            abilities.AttemptSpecialAbility(desiredSpecialAbility, target);
            desiredSpecialAbility = -1;
        }

        void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    target = null;
                    desiredSpecialAbility = keyIndex;
                }
            }
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                wantsToMove = true;
                target = null;
                desiredLocation = destination;
            }
        }

        bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            
            return distanceToTarget <= weaponSystem.GetMaxAttackRange();
        }

        void OnMouseOverEnemy(EnemyAI enemy)
        {
            if (Input.GetMouseButton(0))
            {
                target = enemy.gameObject;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                target = enemy.gameObject;
                desiredSpecialAbility = 0;
            }
        }
    }
}