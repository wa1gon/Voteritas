

namespace TrustedVotingLibraryTest;

using TrustedVoteLibrary.BallotModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;


    [TestClass]
    public class BallotTests
    {
        [TestMethod]
        public void AddItem_ShouldAddSingleVoteItemToBallot()
        {
            // Arrange
            var ballot = new Ballot("Test Ballot");
            var singleVoteItem = new SingleVoteItem("President", new List<string> { "Candidate A", "Candidate B" });

            // Act
            ballot.AddItem(singleVoteItem);

            // Assert
            Assert.AreEqual(1, ballot.Items.Count);
            Assert.AreEqual("President", ballot.Items[0].Title);
        }

        [TestMethod]
        public void SingleVoteItem_ShouldRecordVote()
        {
            // Arrange
            var singleVoteItem = new SingleVoteItem("President", new List<string> { "Candidate A", "Candidate B" });

            // Act
            singleVoteItem.RecordVote(new List<string> { "Candidate A" });

            // Assert
            Assert.AreEqual(1, singleVoteItem.Options["Candidate A"]);
            Assert.AreEqual(0, singleVoteItem.Options["Candidate B"]);
        }

        [TestMethod]
        public void SingleVoteItem_ShouldNotAllowMultipleVotes()
        {
            // Arrange
            var singleVoteItem = new SingleVoteItem("President", new List<string> { "Candidate A", "Candidate B" });

            // Act
            singleVoteItem.RecordVote(new List<string> { "Candidate A", "Candidate B" });

            // Assert
            Assert.AreEqual(0, singleVoteItem.Options["Candidate A"]);
            Assert.AreEqual(0, singleVoteItem.Options["Candidate B"]);
        }

        [TestMethod]
        public void MultiVoteItem_ShouldAllowMultipleVotesUpToLimit()
        {
            // Arrange
            var multiVoteItem = new MultiVoteItem("City Council", new List<string> { "Candidate X", "Candidate Y", "Candidate Z" }, 2);

            // Act
            multiVoteItem.RecordVote(new List<string> { "Candidate X", "Candidate Y" });

            // Assert
            Assert.AreEqual(1, multiVoteItem.Options["Candidate X"]);
            Assert.AreEqual(1, multiVoteItem.Options["Candidate Y"]);
            Assert.AreEqual(0, multiVoteItem.Options["Candidate Z"]);
        }

        [TestMethod]
        public void MultiVoteItem_ShouldNotAllowMoreThanLimit()
        {
            // Arrange
            var multiVoteItem = new MultiVoteItem("City Council", new List<string> { "Candidate X", "Candidate Y", "Candidate Z" }, 2);

            // Act
            multiVoteItem.RecordVote(new List<string> { "Candidate X", "Candidate Y", "Candidate Z" });

            // Assert
            Assert.AreEqual(0, multiVoteItem.Options["Candidate X"]);
            Assert.AreEqual(0, multiVoteItem.Options["Candidate Y"]);
            Assert.AreEqual(0, multiVoteItem.Options["Candidate Z"]);
        }

        [TestMethod]
        public void Ballot_Vote_ShouldRecordVoteForSingleVoteItem()
        {
            // Arrange
            var ballot = new Ballot("Test Ballot");
            var singleVoteItem = new SingleVoteItem("President", new List<string> { "Candidate A", "Candidate B" });
            ballot.AddItem(singleVoteItem);

            // Act
            ballot.Vote("President", new List<string> { "Candidate A" });

            // Assert
            Assert.AreEqual(1, singleVoteItem.Options["Candidate A"]);
        }

        [TestMethod]
        public void Ballot_Vote_ShouldRecordVotesForMultiVoteItem()
        {
            // Arrange
            var ballot = new Ballot("Test Ballot");
            var multiVoteItem = new MultiVoteItem("City Council", new List<string> { "Candidate X", "Candidate Y", "Candidate Z" }, 2);
            ballot.AddItem(multiVoteItem);

            // Act
            ballot.Vote("City Council", new List<string> { "Candidate X", "Candidate Z" });

            // Assert
            Assert.AreEqual(1, multiVoteItem.Options["Candidate X"]);
            Assert.AreEqual(1, multiVoteItem.Options["Candidate Z"]);
            Assert.AreEqual(0, multiVoteItem.Options["Candidate Y"]);
        }
    }
