import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Input } from '../Input';

describe('Input', () => {
  it('renders label, helper text and required indicator', () => {
    render(<Input label="Email" helperText="We never share it" required />);

    expect(screen.getByLabelText(/email/i)).toBeInTheDocument();
    expect(screen.getByText('*')).toHaveClass('text-red-500');
    expect(screen.getByText(/never share/i)).toBeInTheDocument();
  });

  it('shows error message and styles when error prop is provided', () => {
    render(<Input label="Name" error="Required field" />);

    const input = screen.getByLabelText(/name/i);
    expect(input.className).toContain('border-red-500');
    expect(screen.getByText(/required field/i)).toBeInTheDocument();
  });

  it('toggles password visibility when enabled', async () => {
    const user = userEvent.setup();
    render(<Input label="Password" type="password" showPasswordToggle />);

    const input = screen.getByLabelText(/password/i);
    const toggleButton = screen.getByRole('button');

    expect(input).toHaveAttribute('type', 'password');
    await user.click(toggleButton);
    expect(input).toHaveAttribute('type', 'text');
    await user.click(toggleButton);
    expect(input).toHaveAttribute('type', 'password');
  });
});
