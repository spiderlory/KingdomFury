using System.Collections;
using Systems.CombactActionSystem.Model;
using UnityEngine;

namespace Systems.CombactActionSystem.ActionImpl
{
    public class AddSound : IAction
    {
        private AudioClip _audioClip;
        private AudioSource _audioSource;
        private IAction _wrappedAction;

        public AddSound(IAction action, AudioClip audioClip, AudioSource audioSource)
        {
            _audioClip = audioClip;
            _audioSource = audioSource;
            _wrappedAction = action;
        }

        public IEnumerator Execute(CombactContext cbContext)
        {
            _audioSource.Play();
            yield return _wrappedAction.Execute(cbContext);
            _audioSource.Stop();
        }
    }
}