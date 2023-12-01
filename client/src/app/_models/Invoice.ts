export interface Invoice {
    id: number;
    description: string;
    IBAN: string;
    invoiceDate: Date;
    paid: string;
    currency: string;
    amount: string;
    userId: number;
    }
