import { useState } from 'react';
import type { FormEvent } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuthStore } from '../../../shared/stores/authStore';
import { Button, Input, Card } from '../../../shared/ui';

export const RegisterPage = () => {
  const navigate = useNavigate();
  const { register, isLoading, error, clearError } = useAuthStore();
  
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: '',
    confirmPassword: '',
  });

  const [formErrors, setFormErrors] = useState({
    name: '',
    email: '',
    password: '',
    confirmPassword: '',
  });

  const validateForm = (): boolean => {
    const errors = {
      name: '',
      email: '',
      password: '',
      confirmPassword: '',
    };

    if (!formData.name) {
      errors.name = 'Name is required';
    } else if (formData.name.length < 2) {
      errors.name = 'Name must be at least 2 characters';
    }

    if (!formData.email) {
      errors.email = 'Email is required';
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      errors.email = 'Email is invalid';
    }

    if (!formData.password) {
      errors.password = 'Password is required';
    } else if (formData.password.length < 8) {
      errors.password = 'Password must be at least 8 characters';
    } else if (!/(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/.test(formData.password)) {
      errors.password = 'Password must contain uppercase, lowercase, and number';
    }

    if (!formData.confirmPassword) {
      errors.confirmPassword = 'Please confirm your password';
    } else if (formData.password !== formData.confirmPassword) {
      errors.confirmPassword = 'Passwords do not match';
    }

    setFormErrors(errors);
    return !errors.name && !errors.email && !errors.password && !errors.confirmPassword;
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    clearError();

    if (!validateForm()) {
      return;
    }

    try {
      await register(formData);
      navigate('/dashboard');
    } catch (error) {
      // Error is handled by store
      console.error('Registration error:', error);
    }
  };

  const handleChange = (field: string) => (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData((prev) => ({
      ...prev,
      [field]: e.target.value,
    }));
    // Clear error when user starts typing
    if (formErrors[field as keyof typeof formErrors]) {
      setFormErrors((prev) => ({
        ...prev,
        [field]: '',
      }));
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center auth-bg py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8">
        {/* Header */}
        <div className="text-center">
          <div className="flex justify-center mb-4">
            <div className="w-16 h-16 bg-primary-600 rounded-xl flex items-center justify-center">
              <svg className="w-10 h-10 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
            </div>
          </div>
          <h2 className="text-3xl font-bold text-dark-900">
            Create Account
          </h2>
          <p className="mt-2 text-sm text-dark-600">
            Start managing your finances today
          </p>
        </div>

        {/* Register Form */}
        <Card>
          <form className="space-y-6" onSubmit={handleSubmit}>
            {error && (
              <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
                {error}
              </div>
            )}

            <Input
              label="Full Name"
              type="text"
              placeholder="John Doe"
              value={formData.name}
              onChange={handleChange('name')}
              error={formErrors.name}
              required
              autoComplete="name"
            />

            <Input
              label="Email Address"
              type="email"
              placeholder="you@example.com"
              value={formData.email}
              onChange={handleChange('email')}
              error={formErrors.email}
              required
              autoComplete="email"
            />

            <Input
              label="Password"
              name="password"
              type="password"
              placeholder="••••••••"
              value={formData.password}
              onChange={handleChange('password')}
              error={formErrors.password}
              helperText="Must be at least 8 characters with uppercase, lowercase, and number"
              required
              autoComplete="new-password"
              showPasswordToggle
            />

            <Input
              label="Confirm Password"
              name="confirmPassword"
              type="password"
              placeholder="••••••••"
              value={formData.confirmPassword}
              onChange={handleChange('confirmPassword')}
              error={formErrors.confirmPassword}
              required
              autoComplete="new-password"
              showPasswordToggle
            />

            <div className="flex items-start">
              <input
                id="terms"
                type="checkbox"
                required
                className="h-4 w-4 mt-1 text-primary-600 focus:ring-primary-500 border-dark-300 rounded"
              />
              <label htmlFor="terms" className="ml-2 block text-sm text-dark-700">
                I agree to the{' '}
                <a href="#" className="text-primary-600 hover:text-primary-500">
                  Terms of Service
                </a>{' '}
                and{' '}
                <a href="#" className="text-primary-600 hover:text-primary-500">
                  Privacy Policy
                </a>
              </label>
            </div>

            <Button
              type="submit"
              fullWidth
              isLoading={isLoading}
            >
              Create Account
            </Button>
          </form>

          <div className="mt-6">
            <div className="relative">
              <div className="absolute inset-0 flex items-center">
                <div className="w-full border-t border-dark-300" />
              </div>
              <div className="relative flex justify-center text-sm">
                <span className="px-2 bg-white text-dark-500">Already have an account?</span>
              </div>
            </div>

            <div className="mt-6">
              <Link to="/login">
                <Button variant="outline" fullWidth>
                  Sign In
                </Button>
              </Link>
            </div>
          </div>
        </Card>
      </div>
    </div>
  );
};
