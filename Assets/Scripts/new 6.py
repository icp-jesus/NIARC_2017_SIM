# OK, let's build VGG.
tf.reset_default_graph()
input_data = tf.placeholder(tf.float32, (None, 32, 32, 3))
subtracted = input_data - tf.constant(mean_pixel, dtype=tf.float32)
labels = tf.placeholder(tf.int64, (None,))
l2_scale = 5 * 10**-4

# TODO: Build VGG here!
# tf.layers.conv2d(input_layer, filter_count, kernel_size, stride, padding="same")
def Build_Layer(input_layer, num_filters, filter_size):
    layer = tf.layers.conv2d(input_layer, num_filters, filter_size, padding = 'same', 
                             activation=tf.nn.relu, 
                             kernel_initializer=tf.contrib.layers.xavier_initializer_conv2d(),
                             kernel_regularizer=tf.contrib.layers.l2_regularizer(l2_scale))
    return layer
    
with tf.name_scope("Layer_0"):
    layer_0a = Build_Layer(input_data, 64, (3,3))
    layer_0b = Build_Layer(layer_0a, 64, (3,3))
    maxpool_0 = tf.layers.max_pooling2d(layer_0b, 2,2,padding = 'same')

with tf.name_scope("Layer_1"):
    layer_1a = Build_Layer(input_data, 128, (3,3))
    layer_1b = Build_Layer(input_data, 128, (3,3))
    maxpool_1 = tf.layers.max_pooling2d(layer_1b,2,2,padding = 'same')
    
with tf.name_scope("Layer_2"):
    layer_2a = Build_Layer(input_data, 256, (3,3))
    layer_2b = Build_Layer(input_data, 256, (3,3))
    layer_2c = Build_Layer(input_data, 256, (3,3))
    maxpool_2 = tf.layers.max_pooling2d(layer_2c,2,2,padding='same')

with tf.name_scope("Layer_3"):
    layer_3a = Build_Layer(input_data, 512, (3,3))
    layer_3b = Build_Layer(input_data, 512, (3,3))
    layer_3c = Build_Layer(input_data, 512, (3,3))
    maxpool_3 = tf.layers.max_pooling2d(layer_2c,2,2,padding='same')
    
with tf.variable_scope('Layer_4'):
    
    output_logits = layers.conv2d(maxpool_3, 10, 1, activation=None, 
                                  kernel_regularizer=l2_regularizer(l2_scale))
    
    reshaped_output = tf.reshape(output_logits, (-1, 10))
    
    softmax = tf.nn.softmax(reshaped_output)
    
with tf.variable_scope('loss'):
    
    per_example_loss = tf.nn.sparse_softmax_cross_entropy_with_logits(
        labels=labels, logits=reshaped_output)
    
    loss = tf.reduce_mean(per_example_loss) + tf.reduce_sum(tf.get_collection(tf.GraphKeys.REGULARIZATION_LOSSES))