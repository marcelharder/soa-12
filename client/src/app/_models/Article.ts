export interface Article {
    id: number;
    description: string;
    title: string;
    author: string;
    level: number;
    storageUrl?: string;
    ISBN?: string;
}
