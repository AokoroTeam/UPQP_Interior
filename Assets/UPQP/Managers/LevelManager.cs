using Aokoro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UPQP.Features;
using UPQP.Player;


namespace UPQP.Managers
{

    /// <summary>
    /// Initialisation order : 
    /// Level
    /// Player
    /// Entities
    /// Other
    /// </summary>
    [DefaultExecutionOrder(-80)]
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] FeatureDataAsset[] GameFeatures;
        [SerializeField] GameObject PlayerPrefab;
        [SerializeField] Transform SpawnPoint;

        public UPQP_Player Player { get; private set; }


        private Feature[] features;


        protected void Start()
        {
            InitializePlayer();
        }

        private void InitializePlayer()
        {
            //Player creation
            if (UPQP_Player.LocalPlayer == null)
                Player = Instantiate(PlayerPrefab, SpawnPoint.position, SpawnPoint.rotation).GetComponent<UPQP_Player>();
            else
                Player = UPQP_Player.LocalPlayer as UPQP_Player;

            //Features
            int length = GameFeatures.Length;

            features = new Feature[length];
            for (int i = 0; i < length; i++)
                features[i] = GameFeatures[i].GenerateFeature(this);
            
        }

        protected override void OnExistingInstanceFound(LevelManager existingInstance)
        {
            Destroy(gameObject);
        }

    }
}
