using System.Collections;
using UnityEngine;

namespace Systems.CombactActionSystem.ActionImpl
{
    public class AddSound : CombatActionBase
    {
        private AudioClip _audioClip;
        private IAction _wrappedAction;

        public AddSound(IAction action, AudioClip audioClip)
        {
            _audioClip = audioClip;
            _wrappedAction = action;
        }

        protected override IEnumerator Execute(CombatActionContext context)
        {
            AudioSource audioSource = context.PlayerComponents.AudioSource;
            audioSource.Play();
            yield return _wrappedAction.Execute(context);
            audioSource.Stop();
        }
    }
}