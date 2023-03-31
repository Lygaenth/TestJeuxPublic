using NUnit.Framework;
using TestBusiness.Mocks;
using TestJeux.Business.Action;
using TestJeux.SharedKernel.Enums;

namespace TestBusiness
{
	public class TestItemModel
	{
		[TestCase(300, 300, Reactions.None)]
		[TestCase(100, 150, Reactions.Specific)]
		[TestCase(100, 100, Reactions.None)]
		public void TestReactionToMove(int X, int Y, Reactions resultRes)
		{
			var itemModel1 = new MockItemModel(0);
			itemModel1.X = 100;
			itemModel1.Y = 100;
			var itemModel2 = new MockItemModel(1);
			itemModel2.X = X;
			itemModel2.Y = Y;

			var reactionFarAway = itemModel1.ReactMove(itemModel2);
			Assert.That(reactionFarAway.Count, Is.EqualTo(1));
			Assert.That(reactionFarAway[0].ReactionType, Is.EqualTo(resultRes));
			Assert.That(itemModel1.WasRequestedReaction, Is.True);
		}

		[Test]
		public void GetReactionTestWithNoEffect()
		{
			var itemModel1 = new MockItemModel(0);
			var goblinReaction = itemModel1.GetSpecificReaction(2, new MockItemModel(1));
			Assert.That(goblinReaction.Count, Is.EqualTo(0));
		}

		[Test]	
		public void GetReactionTestWithEffect()
		{
			var itemModel1 = new MockItemModel(1);
			var itemModel2 = new MockItemModel(2);
			var mockReaction = itemModel1.GetSpecificReaction(2, itemModel2);
			Assert.That(mockReaction.Count, Is.EqualTo(1));
			Assert.That(mockReaction[0] is EffectAction);
		}
	}
}
