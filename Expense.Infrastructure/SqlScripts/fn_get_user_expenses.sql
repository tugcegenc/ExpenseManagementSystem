CREATE OR REPLACE FUNCTION fn_get_user_expenses(p_user_id BIGINT)
RETURNS TABLE (
    ClaimId BIGINT,
    CategoryName VARCHAR(100),
    Amount NUMERIC(18,2),
    Status TEXT,
    ClaimDate TIMESTAMP WITH TIME ZONE,
    ApprovedOrRejectedDate TIMESTAMP WITH TIME ZONE,
    Description VARCHAR(500)
)
LANGUAGE plpgsql
AS $$
BEGIN
RETURN QUERY
SELECT
    ec."Id",
    cat."Name",
    ec."Amount",
    ec."Status",
    ec."ClaimDate",
    ec."ApprovedOrRejectedDate",
    ec."Description"
FROM dbo."ExpenseClaims" ec
         JOIN dbo."ExpenseCategories" cat ON cat."Id" = ec."ExpenseCategoryId"
WHERE ec."UserId" = p_user_id AND ec."IsActive" = true;
END;
$$;



