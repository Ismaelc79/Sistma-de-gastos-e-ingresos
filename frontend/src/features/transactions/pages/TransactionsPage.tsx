import { Card } from '../../../shared/ui';

export const TransactionsPage = () => {
  return (
    // cooming soon card
    <Card>
        <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-dark-600">Transacciones coming soon</p>
              <p className="text-2xl font-bold text-dark-900 mt-1">$12,543.00</p>
              <p className="text-sm text-green-600 mt-1">+12.5% from last month</p>
            </div>
            <div className="w-12 h-12 bg-primary-100 rounded-lg flex items-center justify-center">
              <svg className="w-6 h-6 text-primary-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
            </div>
          </div>
    </Card>
  );
};
