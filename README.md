# KingdomFury

Questo repository contiene una piccola demo attualmente in fase di sviluppo.

Il progetto ha lo scopo di mostrare le mie capacità e conoscenze tramite
l’implementazione di alcune meccaniche di gioco.

## Overview

Il gioco è un **RPG isometrico con combattimento a turni**, suddiviso in due sezioni principali:
- **Open World**
- **Combat Area**

---

## Open World

In questa sezione il personaggio può muoversi liberamente all’interno di una mappa isometrica,
spostandosi lateralmente e verticalmente.

La mappa contiene diversi elementi interattivi, tra cui:
- Nemici
- Oggetti collezionabili
- Chests
- Vendors
- Interruttori

### Interagibili

L’interazione con gli elementi di gioco segue un approccio classico:
gli oggetti vengono raccolti o attivati nel momento in cui il personaggio interagisce o entra a contatto con essi.

### Nemici

I nemici possono essere di due tipologie:
- **Nascosti**
- **Dinamici**

I nemici nascosti non sono visibili su schermo e sono collocati in aree con
scarsa visibilità, come ad esempio l’erba alta.

I nemici dinamici, invece, sono visibili e possono muoversi all’interno del mondo:
- seguendo percorsi preimpostati
- muovendosi in modo casuale all’interno di una zona assegnata

In entrambi i casi, il combattimento ha inizio quando il giocatore entra
nelle vicinanze del nemico o a diretto contatto con esso.

---

## Combat Area

Quando il giocatore entra in contatto con un nemico, viene spostato
in un’area dedicata al combattimento.

Il combattimento è a turni: ogni personaggio dispone di un proprio turno
durante il quale può eseguire un’azione.

Sono previste le seguenti meccaniche:
- Esecuzione di azioni di combattimento
- Gestione di buff e debuff
  - derivanti dall’equipaggiamento
  - applicati durante lo scontro
- Attacco critico e mancato

Per ogni personaggio sono previste:
- Una lista di **azioni eseguibili**
- **Statistiche di gioco**
  - statistiche base
  - statistiche dinamiche
- **Oggetti equipaggiati**

---

## Stato attuale

### Open World
- **Creazione del mondo di gioco**  
  Implementato un mondo di gioco basato su una tilemap isometrica personalizzata.
  Sono state create tile custom per includere informazioni di percorribilità e
  modificata la *Transparency Sort Mode* per ottenere un corretto effetto tridimensionale.

- **Sistema di movimento del personaggio**  
  Il personaggio può muoversi lateralmente e verticalmente all’interno del mondo di gioco.
  Il sistema è in grado di distinguere altezza e percorribilità delle tile adiacenti per
  determinare se uno spostamento è valido e come deve essere eseguito.

  La logica di movimento è gestita tramite una **matrice tridimensionale 3×3** centrata
  sul personaggio, che viene aggiornata ad ogni spostamento e utilizzata per la validazione
  del movimento.
  
  ![Movimento](https://github.com/user-attachments/assets/1bb01068-d4f9-45b5-a4c6-06dc1825c59f)
---

### Combat System
- **Sistema basato su CombatActions**  
  Implementato un sistema di combattimento basato su **CombatActions**.
  Ogni azione è composta da una sequenza di **Direttive** ed **Eventi** eseguiti in ordine.

  - Le **Direttive** rappresentano azioni atomiche (es. spostamento del personaggio,
    riproduzione di animazioni o timeline).
  - Gli **Eventi** permettono di modificare lo stato del gioco, incluse le statistiche
    dei personaggi.

  Sono attualmente implementati eventi di test utilizzati per validare
  l’architettura del sistema.

- **Esecutore di azioni**  
  Ogni personaggio agisce come semplice esecutore di azioni:
  - Un’azione è definita come una sequenza di **Direttive** ed **Eventi**
  - Le direttive vengono eseguite sequenzialmente per costruire il comportamento finale
  - Ogni personaggio dispone di un modulo **DirectiveExecutor** che espone e implementa
    le direttive specifiche per quel personaggio
    
![Attacco](https://github.com/user-attachments/assets/86550e35-e62a-439e-b118-11a38baac5c0)
---


## Sistemi sviluppati
- **State Machine** per la gestione del comportamento del personaggio nell’open world
- **Movement Validator** basato su una **matrice 3D centrata** per il controllo del movimento
- **Sistema di gestione di azioni ed eventi** per il combattimento
- **Gestione di timeline e segnali** per supportare azioni con timing preciso e
  flussi non strettamente sequenziali tra animazioni ed esecuzione degli eventi

