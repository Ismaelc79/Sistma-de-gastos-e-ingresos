export interface CategorySummaryItem {
  categoryId: number;
  categoryName: string;
  percentage: number;
  percentageRecommended: number;
  totalAmount: number;
}

export interface SummaryReportDto {
  categoryType: string;
  totalPercentage: number;
  totalPercentageRecommended: number;
  totalAmount: number;
  categorySummaryList: CategorySummaryItem[];
}

