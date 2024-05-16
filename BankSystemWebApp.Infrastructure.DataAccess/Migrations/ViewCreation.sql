CREATE OR REPLACE VIEW account_bank_view AS
SELECT a.id AS account_id, b.id AS bank_id
FROM accounts a JOIN banks b ON a.bankid = b.id;