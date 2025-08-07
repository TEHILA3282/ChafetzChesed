export interface LoanRequest {
  amount: number;
  installments: number;
  loanTypeId: number;
  description: string;
  isForApartment: boolean;
  // בהמשך אפשר להוסיף גם guarantors: Guarantor[]
}
