using ProcGen.Graph.Core;
using ProcGen.Materialization;
using ProcGen.Seed;
using UnityEngine;

namespace ProcGen.Graph.Run
{
    public class GenRunner : MonoBehaviour
    {
        [SerializeField]
        private string seed = "bazinga";
        [SerializeField]
        private bool _generateOnStart = true;
        [SerializeField]
        private bool _materializeOnStart = true;
        [SerializeField]
        private bool _logDump = true;
        [SerializeField]
        private bool _isDebug = false;
        [SerializeField]
        private GraphTopology _topology;
        [SerializeField]
        private GridMaterializationSettings _materializationSettings;

        private readonly SeedUtility _seedUtil = new SeedUtility();
        private readonly Initializer _initializer = new Initializer();
        private IInitialGridBuilder _gridBuilder { get; set; }
        private Transform _generatedRoot;

        private void Start()
        {
            if (!_generateOnStart && !_materializeOnStart)
            {
                return;
            }

            if (!EnsureInitialized())
            {
                return;
            }

            if (_isDebug)
            {
                Debug();
            }

            if (_generateOnStart)
            {
                Regenerate();
            }

            if (_materializeOnStart)
            {
                Materialize();
            }
        }

        [ContextMenu("Regenerate")]
        public void Regenerate()
        {
            if (!EnsureInitialized())
            {
                return;
            }

            InitSeedStream(seed, out IRngStream initRng);

            _gridBuilder = new ManualGridBuilder(_initializer.GridRepository, _initializer.PropertyTracker);
            _gridBuilder.Build(initRng, _topology);

            if (_logDump)
            {
                UnityEngine.Debug.Log(_initializer.GridRepository.ToDebugString(_initializer.PropertyTracker), this);
            }
        }

        [ContextMenu("Materialize")]
        public void Materialize()
        {
            if (!EnsureInitialized())
            {
                return;
            }

            if (_materializationSettings == null)
            {
                UnityEngine.Debug.LogError("GridMaterializationSettings is not assigned.", this);
                return;
            }

            if (_generatedRoot != null)
            {
                DestroyImmediate(_generatedRoot.gameObject);
            }

            _generatedRoot = _initializer.Materializer.Materialize(_materializationSettings, transform);
        }

        private bool EnsureInitialized()
        {
            if (_topology == null)
            {
                UnityEngine.Debug.LogError("GraphTopology is not assigned in the Inspector.", this);
                return false;
            }

            _initializer.Initialize(_topology);
            return _initializer.GridRepository != null;
        }

        //Move this to the Initializer
        private void InitSeedStream(string rawSeed, out IRngStream initRng)
        {
            string normalizedSeed = _seedUtil.NormalizeSeed(rawSeed);
            uint baseSeed = _seedUtil.HashSeed(normalizedSeed);
            uint initSeed = _seedUtil.DeriveSubseed(baseSeed, "init");

            UnityEngine.Debug.Log($"Seed='{rawSeed}' normalized='{normalizedSeed}' base={baseSeed} init={initSeed}", this);

            initRng = new UnityRngStream(initSeed);
        }


        [ContextMenu("Debug")]
        public void Debug()
        {
            if (!EnsureInitialized())
            {
                return;
            }

            InitSeedStream(seed, out IRngStream initRng);

            _gridBuilder = new ManualGridBuilder(_initializer.GridRepository, _initializer.PropertyTracker);
            _gridBuilder.Build(initRng, _topology);

            if (_logDump)
            {
                UnityEngine.Debug.Log(_initializer.GridRepository.ToDebugString(_initializer.PropertyTracker), this);
            }
        }
    }
}
