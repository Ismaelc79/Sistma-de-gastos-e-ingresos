import { render, screen } from '@testing-library/react';
import { Card } from '../Card';

describe('Card', () => {
  it('renders children with default padding', () => {
    render(
      <Card>
        <p>Content</p>
      </Card>
    );

    const content = screen.getByText(/content/i);
    expect(content).toBeInTheDocument();
    expect(content.parentElement?.className).toContain('p-6');
  });

  it('applies custom padding and classes', () => {
    render(
      <Card padding="lg" className="shadow-lg">
        <p>Large padding</p>
      </Card>
    );

    const content = screen.getByText(/large padding/i);
    expect(content.parentElement?.className).toContain('p-8');
    expect(content.parentElement?.className).toContain('shadow-lg');
  });
});
