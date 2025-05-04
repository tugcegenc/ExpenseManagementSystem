--DROP FUNCTION IF EXISTS dbo.fn_get_company_expense_status_summary(timestamptz, timestamptz);

CREATE OR REPLACE FUNCTION dbo.fn_get_company_expense_status_summary(
    p_start_date TIMESTAMPTZ,
    p_end_date TIMESTAMPTZ
)
RETURNS TABLE (
    approved_count INTEGER,
    approved_amount NUMERIC,
    rejected_count INTEGER,
    rejected_amount NUMERIC
)
LANGUAGE plpgsql
AS $$
BEGIN
RETURN QUERY
SELECT
    COUNT(*) FILTER (WHERE ec."Status" = 'Approved')::INTEGER AS approved_count,
        COALESCE(SUM(ec."Amount") FILTER (WHERE ec."Status" = 'Approved'), 0) AS approved_amount,
    COUNT(*) FILTER (WHERE ec."Status" = 'Rejected')::INTEGER AS rejected_count,
        COALESCE(SUM(ec."Amount") FILTER (WHERE ec."Status" = 'Rejected'), 0) AS rejected_amount
FROM dbo."ExpenseClaims" ec
WHERE ec."IsActive" = true
  AND ec."ClaimDate" BETWEEN p_start_date AND p_end_date;
END;
$$;
