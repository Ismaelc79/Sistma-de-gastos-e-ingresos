import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Button } from '../Button';

describe('Button', () => {
  it('renders children with default primary styles', () => {
    render(<Button>Click me</Button>);

    const button = screen.getByRole('button', { name: /click me/i });
    expect(button).toBeInTheDocument();
    expect(button.className).toContain('bg-primary-600');
    expect(button).not.toBeDisabled();
  });

  it('respects variant, size and fullWidth props', () => {
    render(
      <Button variant="outline" size="lg" fullWidth className="custom-class">
        Outline
      </Button>
    );

    const button = screen.getByRole('button', { name: /outline/i });
    expect(button.className).toContain('btn-outline');
    expect(button.className).toContain('px-6');
    expect(button.className).toContain('w-full');
    expect(button.className).toContain('custom-class');
  });

  it('shows loading state and prevents multiple submissions', async () => {
    const user = userEvent.setup();
    const handleClick = vi.fn();

    render(
      <Button isLoading onClick={handleClick}>
        Submit
      </Button>
    );

    const button = screen.getByRole('button');
    expect(button).toBeDisabled();
    expect(button).toHaveTextContent(/loading/i);

    await user.click(button);
    expect(handleClick).not.toHaveBeenCalled();
  });
});
