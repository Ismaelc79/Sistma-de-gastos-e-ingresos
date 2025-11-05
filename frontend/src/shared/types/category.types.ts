

export interface CategoryType {
 
    id: string,
    name: string;
  
}


export interface Category {
 
    id: number,
    userId: string,
    name: string,
    type: string,
    createdAt: string
  
}

export interface CategoryInput {
  name: string,
  type: string
}

