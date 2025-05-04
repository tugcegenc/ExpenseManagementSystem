--DROP FUNCTION IF EXISTS dbo.fn_get_personnel_expense_summary(timestamp with time zone, timestamp with time zone);

CREATE OR REPLACE FUNCTION dbo.fn_get_personnel_expense_summary(
    p_start_date timestamp with time zone,
    p_end_date timestamp with time zone
)
RETURNS TABLE (
    UserId bigint,
    FullName text,
    TotalClaimCount bigint,
    TotalClaimAmount numeric,
    ApprovedCount bigint,
    ApprovedAmount numeric,
    RejectedCount bigint,
    RejectedAmount numeric,
    PendingCount bigint,
    PendingAmount numeric
)
LANGUAGE plpgsql
AS $$
BEGIN
RETURN QUERY
SELECT
    u."Id" AS "UserId",
    CONCAT(u."FirstName", ' ', u."LastName") AS "FullName",
    COUNT(ec."Id") AS "TotalClaimCount",
    COALESCE(SUM(ec."Amount"), 0) AS "TotalClaimAmount",
    COUNT(CASE WHEN ec."Status" = 'Approved' THEN 1 END) AS "ApprovedCount",
    COALESCE(SUM(CASE WHEN ec."Status" = 'Approved' THEN ec."Amount" END), 0) AS "ApprovedAmount",
    COUNT(CASE WHEN ec."Status" = 'Rejected' THEN 1 END) AS "RejectedCount",
    COALESCE(SUM(CASE WHEN ec."Status" = 'Rejected' THEN ec."Amount" END), 0) AS "RejectedAmount",
    COUNT(CASE WHEN ec."Status" = 'Pending' THEN 1 END) AS "PendingCount",
    COALESCE(SUM(CASE WHEN ec."Status" = 'Pending' THEN ec."Amount" END), 0) AS "PendingAmount"
FROM dbo."Users" u
         LEFT JOIN dbo."ExpenseClaims" ec ON ec."UserId" = u."Id"
    AND ec."IsActive" = true
    AND ec."ClaimDate" BETWEEN p_start_date AND p_end_date
WHERE u."IsActive" = true
  AND u."Role" = 'Personnel'
GROUP BY u."Id", u."FirstName", u."LastName";
END;
$$;
