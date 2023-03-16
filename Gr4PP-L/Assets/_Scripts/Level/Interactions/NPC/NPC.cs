using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public abstract class NPC : Interactable
    {
        [SerializeField]
        protected NPCData _npc;

        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _npc.Sprite;
        }
    }
}