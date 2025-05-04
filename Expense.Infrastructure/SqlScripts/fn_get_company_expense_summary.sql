
--DROP FUNCTION IF EXISTS dbo.fn_get_company_expense_summary(timestamp with time zone, timestamp with time zone);

CREATE OR REPLACE FUNCTION dbo.fn_get_company_expense_summary(
    p_start_date TIMESTAMPTZ,
    p_end_date TIMESTAMPTZ
)
RETURNS TABLE (
    total_claim_count INTEGER,
    total_claim_amount NUMERIC,
    approved_count INTEGER,
    approved_amount NUMERIC,
    rejected_count INTEGER,
    rejected_amount NUMERIC,
    pending_count INTEGER,
    pending_amount NUMERIC
)
LANGUAGE plpgsql
AS $$
BEGIN
RETURN QUERY
SELECT
    COUNT(ec."Id")::INTEGER AS total_claim_count,
        COALESCE(SUM(ec."Amount"), 0) AS total_claim_amount,

    COUNT(CASE WHEN ec."Status" = 'Approved' THEN 1 END)::INTEGER AS approved_count,
        COALESCE(SUM(CASE WHEN ec."Status" = 'Approved' THEN ec."Amount" END), 0) AS approved_amount,

    COUNT(CASE WHEN ec."Status" = 'Rejected' THEN 1 END)::INTEGER AS rejected_count,
        COALESCE(SUM(CASE WHEN ec."Status" = 'Rejected' THEN ec."Amount" END), 0) AS rejected_amount,

    COUNT(CASE WHEN ec."Status" = 'Pending' THEN 1 END)::INTEGER AS pending_count,
        COALESCE(SUM(CASE WHEN ec."Status" = 'Pending' THEN ec."Amount" END), 0) AS pending_amount
FROM dbo."ExpenseClaims" ec
WHERE ec."IsActive" = true
  AND ec."ClaimDate" BETWEEN p_start_date AND p_end_date;
END;
$$;
