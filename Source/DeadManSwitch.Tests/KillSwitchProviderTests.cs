using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DeadManSwitch.Action.KillSwitches;
using DeadManSwitch.Data;
using DeadManSwitch.Data.TestRepository;
using DeadManSwitch.Data.TestRepository.Tables;
using Microsoft.Practices.Unity;
using System.Linq;

namespace DeadManSwitch.Tests
{
    [TestClass]
    public class KillSwitchProviderTests
    {
        private static int RuleIdentity = 1;

        private KillSwitchRule CreateRule(int killSwitchId, int limit, TimeSpan period, bool activateKillSwitch = true)
        {
            KillSwitchRule rule = new KillSwitchRule();
            rule.Id = RuleIdentity++;

            rule.ActivateKillSwitchIfSatisfied = activateKillSwitch;
            rule.IsActive = true;
            rule.KillSwitchId = killSwitchId;
            rule.Limit = limit;
            rule.Period = period;

            return rule;
        }

        private IUnityContainer GetContainerForTest(RepositoryContext context)
        {
            return TestIoCConfig.BuildContainer(context);
        }

        [TestMethod]
        public void KillSwitchProviderOutgoingTextMsg_RuleIsTriggered_KillSwitchIsEngaged()
        {
            //Arrange
            var context = new RepositoryContext();

            ActionType actionType = ActionType.TextMessage;
            ActionDirection direction = ActionDirection.Outgoing;

            //Create rule to trigger kill switch if 2 outgoing txt msgs in 1 minute
            var existingKillSwitch = context.KillSwitches.Single(r => r.ActionType == actionType && r.Direction == direction);
            context.KillSwitchRules.Add(this.CreateRule(existingKillSwitch.Id, 2, new TimeSpan(0, 1, 0), true));

            //Fake 2 outgoing msgs
            context.EscalationActionLogs.Add(new EscalationActionLogTableRow(actionType, direction));
            context.EscalationActionLogs.Add(new EscalationActionLogTableRow(actionType, direction));

            //Setup container with current repository context
            IUnityContainer container = this.GetContainerForTest(context);

            //Act
            DeadManSwitch.Providers.KillSwitchProvider cut = new Providers.KillSwitchProvider(container);
            KillSwitch result = cut.CheckKillSwitch(actionType, direction);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsEngaged);
        }

        [TestMethod]
        public void KillSwitchProviderOutgoingTextMsg_RuleIsTriggered_KillSwitchIsNotEngaged()
        {
            //Arrange
            var context = new RepositoryContext();

            ActionType actionType = ActionType.TextMessage;
            ActionDirection direction = ActionDirection.Outgoing;

            //Create rule to check for 2 outgoing txt msgs in 1 minute, but don't trigger kill switch
            var existingKillSwitch = context.KillSwitches.Single(r => r.ActionType == actionType && r.Direction == direction);
            context.KillSwitchRules.Add(this.CreateRule(existingKillSwitch.Id, 2, new TimeSpan(0, 1, 0), false));

            //Fake 2 outgoing msgs
            context.EscalationActionLogs.Add(new EscalationActionLogTableRow(actionType, direction));
            context.EscalationActionLogs.Add(new EscalationActionLogTableRow(actionType, direction));

            //Setup container with current repository context
            IUnityContainer container = this.GetContainerForTest(context);

            //Act
            DeadManSwitch.Providers.KillSwitchProvider cut = new Providers.KillSwitchProvider(container);
            KillSwitch result = cut.CheckKillSwitch(actionType, direction);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsEngaged);
        }

    }
}
