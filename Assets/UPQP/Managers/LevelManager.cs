using Aokoro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UPQP.Features;
using UPQP.Player;


namespace UPQP.Managers
{

    
    [DefaultExecutionOrder(-80)]
    public class LevelManager : Singleton<LevelManager>
    {
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

        [SerializeField] FeatureDataAsset[] GameFeatures;
        [SerializeField] GameObject PlayerPrefab;
        [SerializeField] Transform SpawnPoint;

        public UPQP_Player Player { get; private set; }

        protected void Start()
        {
            LevelInitiationPhase = levelPhase;
            InitializeLevel();
            LevelInitiationPhase = playerPhase;
            InitializePlayer();
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

    }
}
