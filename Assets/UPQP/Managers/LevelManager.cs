using Aokoro;
using Aokoro.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UPQP.Features;
using UPQP.Player;


namespace UPQP.Managers
{


    [DefaultExecutionOrder(-80)]
    [AddComponentMenu("UPQP/Managers/LevelManager")]
    public class LevelManager : Singleton<LevelManager>
    {

        public GameUIManager mainUI;

        /// Initialisation order : 
        /// Level
        /// Player
        /// Entities
        /// Other
        public string LevelInitiationPhase { get; private set; } = "None";

        public const string levelPhase = "levelPhase";
        public const string playerPhase = "playerPhase";
        public const string EntitiesPhase = "entitiesPhase";
        public const string DonePhase = "Done";


        public static event Action<UPQP_Player> OnPlayerIsCreated;

        [SerializeField] FeatureDataAsset[] GameFeatures;
        [SerializeField] GameObject PlayerPrefab;
        [SerializeField] Transform SpawnPoint;

        public UPQP_Player Player { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            GameUIManager.MainUI = mainUI;
        }

        protected void Start()
        {
            LevelInitiationPhase = levelPhase;
            InitializeLevel();
            LevelInitiationPhase = playerPhase;
            InitializePlayer();
            OnPlayerIsCreated?.Invoke(Player);
            LevelInitiationPhase = DonePhase;

        }

        protected virtual void InitializeLevel()
        {

        }

        protected virtual void InitializePlayer()
        {
            //Player creation
            if (UPQP_Player.LocalPlayer == null)
                Player = Instantiate(PlayerPrefab, SpawnPoint.position, SpawnPoint.rotation).GetComponent<UPQP_Player>();
            else
                Player = UPQP_Player.LocalPlayer as UPQP_Player;

            Player.OnAwake();

        }

        public Feature[] CreateLevelFeatures()
        {
            int length = GameFeatures.Length;

            var features = new Feature[length];
            for (int i = 0; i < length; i++)
                features[i] = GameFeatures[i].GenerateFeature(this);

            return features;
        }

        protected override void OnExistingInstanceFound(LevelManager existingInstance)
        {
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            OnPlayerIsCreated = null;
        }
    }
}
