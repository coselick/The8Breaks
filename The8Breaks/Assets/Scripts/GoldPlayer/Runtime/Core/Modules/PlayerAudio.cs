using Q17pD;
using UnityEngine;

namespace Hertzole.GoldPlayer
{
    [System.Serializable]
    public sealed class PlayerAudio : PlayerModule
    {
        [Header("Audio Handler")]
        [SerializeField]
        private AudioHandler _audioHandler;

        [Header("Settings")]
        [SerializeField]
        private bool enableAudio = true;

        [SerializeField]
        private bool unscaledTime = false;

        [SerializeField]
        private bool basedOnHeadBob = true;

        [SerializeField]
        private float stepTime = 1.5f;

        [Header("Footsteps")]
        [SerializeField]
        private AudioClip[] walkFootsteps;

        [SerializeField]
        private AudioClip[] runFootsteps;

        [SerializeField]
        private AudioClip[] crouchFootsteps;

        [Header("Jump & Landing")]
        [SerializeField]
        private AudioClip[] jumpingSounds;

        [SerializeField]
        private AudioClip[] landingSounds;

        private float nextStepTime;
        private float stepCycle;

        private bool previouslyGrounded = true;

        private AudioSource currentFootstepSource;
        private AudioSource currentJumpSource;
        private AudioSource currentLandSource;

        public AudioHandler AudioHandler
        {
            get => _audioHandler;
            set => _audioHandler = value;
        }
        public bool EnableAudio
        {
            get => enableAudio;
            set => enableAudio = value;
        }

        public bool UnscaledTime
        {
            get => unscaledTime;
            set => unscaledTime = value;
        }

        public bool BasedOnHeadBob
        {
            get => basedOnHeadBob;
            set => basedOnHeadBob = value;
        }

        public float StepTime
        {
            get => stepTime;
            set => stepTime = value;
        }

        protected override void OnInitialize()
        {
            if (_audioHandler == null)
            {
                Debug.LogError(
                    "PlayerAudio: AudioHandler is not assigned!",
                    PlayerController
                );
            }

            previouslyGrounded = PlayerController.Movement.IsGrounded;
        }

        public override void OnUpdate(
            float deltaTime,
            float unscaledDeltaTime)
        {
            if (!enableAudio || _audioHandler == null)
                return;

            if (unscaledTime)
                deltaTime = unscaledDeltaTime;

            DoStepCycle(deltaTime);
            HandleAudio();
        }

        private void DoStepCycle(float deltaTime)
        {
            if (basedOnHeadBob &&
                PlayerController.HeadBob.EnableBob)
            {
                stepCycle = PlayerController.HeadBob.BobCycle;
            }
            else
            {
                DoStepCycleMath(deltaTime);
            }
        }

        private void DoStepCycleMath(float deltaTime)
        {
            if (CharacterController == null)
                return;

            Vector3 velocity = CharacterController.velocity;

            float flatVelocity = new Vector3(
                velocity.x,
                0,
                velocity.z
            ).magnitude;

            float strideLengthen =
                1f + flatVelocity * 0.3f;

            stepCycle +=
                (flatVelocity / strideLengthen) *
                (deltaTime / Mathf.Max(0.01f, stepTime));
        }

        private void HandleAudio()
        {
            bool grounded =
                PlayerController.Movement.IsGrounded;

            if (grounded)
            {
                if (!previouslyGrounded)
                {
                    PlayLandSound();

                    nextStepTime = stepCycle + 0.5f;
                }
                else if (stepCycle > nextStepTime)
                {
                    PlayFootstepSound();

                    nextStepTime = stepCycle + 0.5f;
                }
            }
            else if (previouslyGrounded)
            {
                PlayJumpSound();
            }

            previouslyGrounded = grounded;
        }

        public void PlayFootstepSound()
        {
            if (!enableAudio)
                return;

            AudioClip[] clips;
            float volume = 1f;

            if (PlayerController.Movement.IsRunning) { clips = runFootsteps; volume = 0.5f; }
            else if (PlayerController.Movement.IsCrouching) { clips = crouchFootsteps; volume = 0.15f; }
            else { clips = walkFootsteps; volume = 0.5f; }

            currentFootstepSource = PlayRandomSound(clips, volume);
        }

        public void PlayJumpSound()
        {
            if (!enableAudio)
                return;

            currentJumpSource =
                PlayRandomSound(jumpingSounds);
        }

        public void PlayLandSound()
        {
            if (!enableAudio)
                return;

            currentLandSource =
                PlayRandomSound(landingSounds);
        }

        private AudioSource PlayRandomSound(AudioClip[] clips, float volume = 1f)
        {
            if (_audioHandler == null || clips == null || clips.Length == 0) return null;
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            if (clip == null) return null;
            return _audioHandler.PlaySound(SoundType.SFX, clip, volume);
        }

        public void StopFootstepSound()
        {
            StopSound(ref currentFootstepSource);
        }

        public void StopJumpSound()
        {
            StopSound(ref currentJumpSource);
        }

        public void StopLandSound()
        {
            StopSound(ref currentLandSource);
        }

        public void StopAllSounds()
        {
            StopFootstepSound();
            StopJumpSound();
            StopLandSound();
        }
        public void ResetAudio()
        {
            stepCycle = 0f;
            nextStepTime = 0f;

            previouslyGrounded = PlayerController.Movement.IsGrounded;
        }

        private void StopSound(ref AudioSource source)
        {
            if (_audioHandler == null || source == null)
                return;

            if (source.isPlaying)
            {
                _audioHandler.StopSound(source);
            }

            source = null;
        }
    }
}