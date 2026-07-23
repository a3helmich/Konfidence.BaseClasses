using System;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.Mail.UnitTest;

[TestClass]
public class BaseMailSenderTests
{
    [TestMethod]
    public void SendEmail_WithNonExistentAttachmentFile_ReturnsFalseInsteadOfThrowing()
    {
        // Arrange
        BaseMailSender mailSender = new("from@example.com", "smtp.invalid.example", "user", "password");
        string nonExistentFile = Path.Combine(Path.GetTempPath(), $"DoesNotExist_{Guid.NewGuid():N}.txt");

        // Act
        bool result = false;
        Action action = () => result = mailSender.SendEmail("to@example.com", "subject", "body", false, nonExistentFile);

        // Assert
        // Before the fix, constructing the Attachment happened before the try block, so a bad
        // attachment path threw straight out of SendEmail instead of returning false like every
        // other failure mode.
        action.Should().NotThrow();
        result.Should().BeFalse();
    }
}
