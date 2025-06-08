using System.ComponentModel;

namespace Acme.OnlineCourses.Students;

public enum TestStatus
{
    [Description("Not Taken")]
    NotTaken = 0,
    
    [Description("Taken")]
    Taken = 1,
    
    [Description("Passed")]
    Passed = 2,
    
    [Description("Failed")]
    Failed = 3
}

public enum PaymentStatus
{
    [Description("Not Paid")]
    NotPaid = 0,
    
    [Description("Paid")]
    Paid = 1
}

public enum AccountStatus
{
    [Description("Not Sent")]
    NotSent = 0,
    
    [Description("Sent")]
    Sent = 1,

    [Description("suspended")]
    Suspended = 2,
} 