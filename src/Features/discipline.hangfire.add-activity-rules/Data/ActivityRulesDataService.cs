using Dapper;
using discipline.hangfire.add_activity_rules.Data.Abstractions;
using discipline.hangfire.add_activity_rules.DTOs;
using discipline.hangfire.shared.abstractions.DataAccess;
using discipline.hangfire.shared.abstractions.Identifiers;

namespace discipline.hangfire.add_activity_rules.Data;

internal sealed class ActivityRulesDataService(
    IDbContext context) : IActivityRulesDataService
{
    public async Task AddActivityRule(ActivityRuleDto activityRuleDto, UserId userId, DateTime updatedAt)
    {
        using var connection = context.GetConnection();
        connection.Open();

        const string sql = """
                           INSERT INTO centre."ActivityRules" ("activity_rule_id", "user_id", "mode", "selected_days", "updated_at")
                           VALUES (@ActivityRuleId, @UserId, @Mode, @SelectedDays, @UpdatedAt);
                           """;
        
        await connection.ExecuteAsync(sql, new
        {
            ActivityRuleId = activityRuleDto.ActivityRuleId.Value.ToString(),  
            UserId = userId.Value.ToString(),
            Mode = activityRuleDto.Mode,
            SelectedDays = activityRuleDto.SelectedDays?.ToArray(),
            UpdatedAt = updatedAt
        });
    }
}