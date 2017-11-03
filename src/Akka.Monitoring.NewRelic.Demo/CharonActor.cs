﻿using Akka.Actor;
using Akka.Dispatch.SysMsg;
using Akka.Event;

namespace Akka.Monitoring.NewRelic.Demo
{
    public class CharonActor : ReceiveActor
    {
        public class ImYourMoon { }

        public class WeGoRoundAndRound { }

        public CharonActor()
        {
            Context.IncrementActorCreated();

            Receive<IActorRef>(actor =>
            {
                Context.IncrementMessagesReceived();
                actor.Tell(new ImYourMoon());
            });

            Receive<PlutoActor.YoureMyMoon>(_ =>
            {
                Context.IncrementMessagesReceived();
                Sender.Tell(new WeGoRoundAndRound());
                Context.GetLogger().Warning("From out here...");
                Self.Tell(PoisonPill.Instance);
            });

            Receive<Stop>(_ =>
            {
                Context.IncrementActorStopped();
            });
        }
    }
}