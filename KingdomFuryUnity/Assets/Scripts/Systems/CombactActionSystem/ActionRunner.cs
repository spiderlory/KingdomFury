using System;
using System.Collections;
using System.Collections.Generic;
using Systems.CombactActionSystem.ActionImpl;
using Systems.CombactActionSystem.Model;
using UnityEngine;
using UnityEngine.Playables;

namespace Systems.CombactActionSystem
{
    public class ActionRunner : MonoBehaviour
    {
        private List<IAction> _actionsList;
        public GameObject enemy;
        public PlayableAsset asset;

        private void Start()
        {
            _actionsList = new List<IAction>();

            IAction action = new MoveTo(ctx => ctx.EnemyTargets[0].transform.position);
            action = new SetAnimatorBool(action, GetComponent<Animator>(), "IsWalking");
            
            _actionsList.Add(action);
            _actionsList.Add(new TimelineAction(GetComponent<PlayableDirector>(), asset));
            Vector2 startPosition = transform.position;
            action = new MoveTo(ctx => startPosition);
            
            _actionsList.Add(new SetAnimatorBool(action, GetComponent<Animator>(), "IsWalking"));
        }

        public void Test()
        {
            StartCoroutine(ExecuteActions());
        }

        public IEnumerator ExecuteActions()
        {
            print("Playing action");
            List<GameObject> enemies = new List<GameObject>();
            enemies.Add(enemy);
            CombactContext combactContext = new CombactContext(gameObject, enemies, enemies);
            
            foreach (IAction action in _actionsList)
            {
                yield return StartCoroutine(action.Execute(combactContext));
            }
        }
    }
}

///
/// CombactContext:
///     alliesList
///         ally1 ->
///             stats
///             currentStats
///             modifierList
///     enemiesList
///     
///
/// Come passare info alle azioni:
///     contesto -> devo sapere in anticipo cosa devo fare, serve una lista di 
///     view -> chiede le info alla view e aspetta una risposta
///
///     Ogni azione ha bisogno di un init, nell'init
///
///
/// Eventi:
///     X danneggia [Y] <- Azione, Debuff            Damage(Targets, value)
///     X cura [Y] <- Azione, Debuff                 Heal(Targets, value)
///     X applica un modificatore [Y] <- Azione      ApplyModifier(Targets, Modifier)
///
/// Ogni giocatore gestisce i propri buff/debuff?
/// L'azione Damage potrebbe essere un evento, non un azione
///
///
///
///
///
///
/// Problemi:
///     Ogni personaggio gestisce personalmente gli eventi?
///     
///     Le azioni prendono inputs?
///         no -> qualcos'altro deve gestire quanto danno fare, dove muoversi
///               esempio: Damage(1, 0.25) -> Danneggia il primo bersaglio, 0.25 dell'attacco del giocatore
///                        MoveTo(1) -> Muoviti verso il bersaglio 1
///               Questi metodi devono probabilmente stare sul giocatore stesso, sul giocatore avrei: struttura dati per le stats e le azioni, action runner -> azione -> interfaccia per l'esecuzione degli eventi
///         si -> metto a disposizione un contesto con info all'interno
///               context: enemyTargets, allyTargets, damageMultiplier, healMultiplier, player (stat, currentStats, etc)
///               heal, damage, applyModifier
/// 
///               Posso applicare modificatori direttamente sull'azione (tiene traccia delle modifiche fino al momento del danno, le percentuali vengono sommate tra loro non moltiplicate) perché possono modificare il contesto
///               In alternativa i modificatori devono essere applicati prima di eseguire l'azione
///    MultiplyDamage(Action, context): context.damageMultiplier +- valore
///
///    ListaModificatori = [Buff1(timeLeft), 2(), 3]
///    
///    Buff3(Buff1(ACTION)))
///               Per non istanziare troppi oggetti posso mantenere il puntamento all'ultimo buff della fila e appendere direttamente li l'evento, senza dover reistanziare tutto da capo.
///               Se faccio funzionare i buff come una lista posso rimuovere un buff semplicemente conoscendolo
/// 
///         A prescindere, devo sapere in anticipo cosa serve all'azione, nello specifico se ha bersagli, quanti e se sono alleati o nemici
///
///
///    Heal(target, value): target.Heal(value)
///    Damage(target, additionalDamage, multiplier): target.Damage(BaseDamage * multiplier)
///
///    TakeDamage(): damage * (1 - def * 0.2)
///    Heal
///    ApplyModifier
///
///    Giocatore -> Azione -> Giocatore
///
///
///
///
///
/// Come risolvere le timeLine con eventi:
///     Le timeline possono inviare segnali o chiamare metodi
///
///
///
///
///
///
///
/// Assunzioni:
///     Le azioni devono essere atomiche
///     Le azioni decorator devono solo aggiungere pezzi in più -> attivare un'animazione, aggiungere un suono in loop
///     Qualsiasi cosa che può essere fatta in sequenza deve essere fatta in sequenza -> Bevo la pozione e mi curo -> azioni separate
///
///     Le timeline devono essere azioni complete. L'azione deve contenere tutta la logica che serve.
///     Azioni condizionali ad esempio possono prendere in input azioni. Tipo: attaccaOgniNemicoUnaVolta(Action) -> per ogni nemico in context, si muove dal nemico e attiva l'azione
///     Questo layer logico deve essere associato al runner. Ogni azione avrà un proprio runner, oppure un azione che si adatta.
///
///     La parte logica la preferisco staccata perché risulta complicato definire azioni complesse
///
///     Per sincronizzare timeline e eventi facciamo questo:
///         Lista di Action
///         Oggetto Listner
///         Evento NextAction
///
///         Quando viene chiamato NextAction il Listner esegue l'azione.
///         Come associo il listner all'evento/segnale?
///
///
/// 
