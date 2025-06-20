namespace ann_class
{
    using System;
    using vector_class;
    using minimiser_class;

    // Neural network class
    public class ANN
    {

        // Number of hidden neurons
        public int n;

        // Gaussian activation function, its first two derivatives, and its antiderivative
        public Func<double, double> f = (x) => x * Math.Exp(-x * x);
        public Func<double, double> df = (x) => (1 - 2 * x * x) * Math.Exp(-x * x);
        public Func<double, double> d2f = (x) => 2 * x * (2 * x * x - 3) * Math.Exp(-x * x);
        public Func<double, double> F = (x) => 0.5 * (1 - Math.Exp(-x * x));

        // Network parameters
        public vector p;

        // Constructor
        public ANN(int n)
        {
            // Number of neurons
            this.n = n;

            // Parameters (three per neuron)
            p = new vector(3 * n);
            for (int i = 0; i < p.size; i++) p[i] = 1.0;
        }

        // Evaluate the network at a given point x
        public double response(double x, vector p = null)
        {
            if (p == null) p = this.p;
            double sum = 0.0;
            for (int i = 0; i < n; i++)
            {
                double a = p[3 * i];
                double b = p[3 * i + 1];
                double w = p[3 * i + 2];
                sum += w * f((x - a) / b);
            }
            return sum;
        }

        // First derivative of the network response
        public double derivative(double x, vector p = null)
        {
            if (p == null) p = this.p;
            double sum = 0.0;
            for (int i = 0; i < n; i++)
            {
                double a = p[3 * i];
                double b = p[3 * i + 1];
                double w = p[3 * i + 2];

                double u = (x - a) / b;   // u-substitution of inside
                sum += w * df(u) / b;      // derivative of the activation function
            }
            return sum;
        }

        // Second derivative of the network response
        public double second_derivative(double x, vector p = null)
        {
            if (p == null) p = this.p;
            double sum = 0.0;
            for (int i = 0; i < n; i++)
            {
                double a = p[3 * i];
                double b = p[3 * i + 1];
                double w = p[3 * i + 2];

                double u = (x - a) / b;   // u-substitution of inside
                sum += w * d2f(u) / (b * b);   // second derivative of the activation function
            }
            return sum;
        }

        // Antiderivative of the network response
        public double antiderivative(double x, vector p = null)
        {
            if (p == null) p = this.p;
            double sum = 0.0;
            for (int i = 0; i < n; i++)
            {
                double a = p[3 * i];
                double b = p[3 * i + 1];
                double w = p[3 * i + 2];

                double u = (x - a) / b;   // u-substitution of inside
                sum += w * b * F(u);      // antiderivative of the activation function
            }
            return sum;
        }

        // Analytical cost function gradient (tried with numerical gradient from minimisation ∞ times without convergence)
        // The analytical newton method expects a Func<vector, vector> at each point (x,y), so we need a nested function
        public Func<vector, vector> cost_gradient(vector xs_train, vector ys_train)
        {
            Func<vector, vector> gradient = (param) =>
            {
                vector grad = new vector(3 * n);
                for (int i = 0; i < param.size; i += 3)
                {
                    double a = param[i];
                    double b = param[i + 1];
                    double w = param[i + 2];
                    if (Math.Abs(b) < 1e-10) b = 1e-10; // Avoid division by zero

                    double a_grad = 0.0;
                    double b_grad = 0.0;
                    double w_grad = 0.0;

                    for (int j = 0; j < xs_train.size; j++)
                    {
                        double f_val = f((xs_train[j] - a) / b);
                        double df_val = df((xs_train[j] - a) / b);
                        double chain_rule_factor = 2.0 / xs_train.size * (response(xs_train[j], param) - ys_train[j]);
                        a_grad += -chain_rule_factor * w * df_val / b;
                        b_grad += -chain_rule_factor * w * df_val * (xs_train[j] - a) / (b * b);
                        w_grad += chain_rule_factor * f_val;
                    }
                    grad[i] = a_grad;
                    grad[i + 1] = b_grad;
                    grad[i + 2] = w_grad;
                }
                return grad;
            };
            return gradient;
        }

        // Training the neural network with given training data
        public void train(vector xs_train, vector ys_train)
        {
            // Cost function
            Func<vector, double> cost_func = (params_train) =>
            {
                double cost = 0.0;
                for (int i = 0; i < xs_train.size; i++)
                {
                    double y_pred = response(xs_train[i], params_train);
                    double y_true = ys_train[i];
                    cost += (y_pred - y_true) * (y_pred - y_true);
                }
                return cost / xs_train.size; // Normalized cost
            };
            Func<vector, vector> gradient = cost_gradient(xs_train, ys_train);
            var result = Minimiser.newton_analytical(cost_func, gradient, p, acc: 1e-3, max_iter: 1000);
            p = result.Item1;
            Console.WriteLine($"Number of iterations: {result.Item2}");
        }

    }
}