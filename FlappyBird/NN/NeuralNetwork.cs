using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.Visual.NN
{
    public class ActivationFunction
    {
        public ActivationFunction(Matrix.MapFunction func, Matrix.MapFunction dfunc)
        {
            this.func = func;
            this.dfunc = dfunc;
        }

        public Matrix.MapFunction func { get; }
        public Matrix.MapFunction dfunc { get; }
    }
    public class NeuralNetwork
    {
        private static readonly ActivationFunction sigmoid = new ActivationFunction(
            x => 1 / (1 + Math.Exp(-x)),
            y => y * (1 - y)
        );
        private static readonly ActivationFunction tanh = new ActivationFunction(
          x => Math.Tanh(x),
          y => 1 - (y * y)
        );
        private ActivationFunction activation_function = sigmoid;
        
        public double learning_rate { get;  set; }
        public int input_nodes { get;  set; }
        public int hidden_nodes { get;  set; }
        public int output_nodes { get;  set; }

        public Matrix weights_ih { get;  set; }
        public Matrix weights_ho { get;  set; }
        public Matrix bias_h { get;  set; }
        public Matrix bias_o { get;  set; }

        public NeuralNetwork(int inNodes, int hidNodes, int outNodes)
        {
            this.input_nodes = inNodes;
            this.hidden_nodes = hidNodes;
            this.output_nodes = outNodes;

            this.weights_ih = new Matrix(this.hidden_nodes, this.input_nodes).randomize();
            this.weights_ho = new Matrix(this.output_nodes, this.hidden_nodes).randomize();

            this.bias_h = new Matrix(this.hidden_nodes, 1).randomize();
            this.bias_o = new Matrix(this.output_nodes, 1).randomize();

            // TODO: copy these as well
            this.setLearningRate();
            this.setActivationFunction(sigmoid);
        }

        public NeuralNetwork(NeuralNetwork in_nodes)
        {
            var a = in_nodes;
            this.input_nodes = a.input_nodes;
            this.hidden_nodes = a.hidden_nodes;
            this.output_nodes = a.output_nodes;

            this.weights_ih = a.weights_ih.copy();
            this.weights_ho = a.weights_ho.copy();

            this.bias_h = a.bias_h.copy();
            this.bias_o = a.bias_o.copy();

            // TODO: copy these as well
            this.setLearningRate();
            this.setActivationFunction(sigmoid);
        }
        private NeuralNetwork()
        {

        }


        public double[] predict(double[] input_array)
        {

            // Generating the Hidden Outputs
            var inputs = Matrix.fromArray(input_array);
            var hidden = this.weights_ih.multiply(inputs);
            hidden = hidden.add(this.bias_h);
            // activation function!
            hidden = hidden.map(this.activation_function.func);

            // Generating the output's output!
            var output = this.weights_ho.multiply(hidden);
            output = output.add(this.bias_o);
            output = output.map(this.activation_function.func);

            // Sending back to the caller!
            return output.toArray();
        }
        private void setLearningRate(double learning_rate = 0.1)
        {
            this.learning_rate = learning_rate;
        }

        private void setActivationFunction(ActivationFunction func)
        {
            this.activation_function = func;
        }

        private void train(double[] input_array, double[] target_array)
        {
            // Generating the Hidden Outputs
            var inputs = Matrix.fromArray(input_array);
            var hidden = this.weights_ih.multiply(inputs);
            hidden = hidden.add(this.bias_h);
            // activation function!
            hidden = hidden.map(this.activation_function.func);

            // Generating the output's output!
            var outputs = this.weights_ho.multiply(hidden);
            outputs = outputs.add(this.bias_o);
            outputs = outputs.map(this.activation_function.func);

            // Convert array to matrix object
            var targets = Matrix.fromArray(target_array);

            // Calculate the error
            // ERROR = TARGETS - OUTPUTS
            var output_errors = targets.substract(outputs);

            // var gradient = outputs * (1 - outputs);
            // Calculate gradient
            var gradients = outputs.map(this.activation_function.dfunc);
            gradients = gradients.multiply(output_errors);
            gradients = gradients.multiply(this.learning_rate);


            // Calculate deltas
            var hidden_T = hidden.transpose();
            var weight_ho_deltas = gradients.multiply(hidden_T);

            // Adjust the weights by deltas
            this.weights_ho = this.weights_ho.add(weight_ho_deltas);
            // Adjust the bias by its deltas (which is just the gradients)
            this.bias_o = this.bias_o.add(gradients);

            // Calculate the hidden layer errors
            var who_t = this.weights_ho.transpose();
            var hidden_errors = who_t.multiply(output_errors);

            // Calculate hidden gradient
            var hidden_gradient = hidden.map(this.activation_function.dfunc);
            hidden_gradient = hidden_gradient.multiply(hidden_errors);
            hidden_gradient = hidden_gradient.multiply(this.learning_rate);

            // Calcuate input->hidden deltas
            var inputs_T = inputs.transpose();
            var weight_ih_deltas = hidden_gradient.multiply(inputs_T);

            this.weights_ih = this.weights_ih.add(weight_ih_deltas);
            // Adjust the bias by its deltas (which is just the gradients)
            this.bias_h = this.bias_h.add(hidden_gradient);

            // outputs.print();
            // targets.print();
            // error.print();
        }

        // Accept an arbitrary function for mutation
        public void mutate(Matrix.MapFunction func)
        {
            this.weights_ih = this.weights_ih.map(func);
            this.weights_ho = this.weights_ho.map(func);
            this.bias_h = this.bias_h.map(func);
            this.bias_o = this.bias_o.map(func);
        }
        public void mutate(Matrix.MapFunctionWithIndices func)
        {
            this.weights_ih = this.weights_ih.map(func);
            this.weights_ho = this.weights_ho.map(func);
            this.bias_h = this.bias_h.map(func);
            this.bias_o = this.bias_o.map(func);
        }

    }
}
