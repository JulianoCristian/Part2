﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Quests
{
    [RequireComponent(typeof(Character))]
    public class Escort : QuestCompletion
    {
        // configuration parameters, consider SO

        // private instance variables for state
        bool isEscorting = false;

        void Update()
        {
            var questStarted = questToComplete.QuestState == QuestState.Started;
            if (questStarted && isEscorting == false)
            {
                var player = GameObject.FindWithTag("Player");
                StartCoroutine(FollowPlayer(player));
                isEscorting = true;
            }
        }

        private IEnumerator FollowPlayer(GameObject player)
        {
            while (true)
            {
                GetComponent<Character>().SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        // Detect when at destination
        // Do something
    }
}