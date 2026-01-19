using UnityEngine;
using UnityEngine.Playables;

namespace Systems.CombactActionSystem
{
    public class PlayerComponentsContext
    {
        public Animator Animator { get; }
        public AudioSource AudioSource { get; }
        public PlayableDirector PlayableDirector { get; }
        
        public PlayerComponentsContext(GameObject currentPlayer)
        {
            Animator = currentPlayer.GetComponent<Animator>();
            AudioSource = currentPlayer.GetComponent<AudioSource>();
            PlayableDirector = currentPlayer.GetComponent<PlayableDirector>();
        }
    }
}